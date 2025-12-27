using System;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public static class RentalRules
    {
        public class EligibilityResult
        {
            public bool Allowed { get; set; }
            public string Reason { get; set; }
        }

        public static EligibilityResult CheckCustomerEligibility(string carPlate, int customerId)
        {
            string cs = Functions.GetConnectionString();
            int classMinYears = 0;
            bool isPremium = false;
            decimal? carPrice = null;
            string currentCarClass = "";

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                var cmd = new SqlCommand(@"
SELECT ISNULL(cc.MinExperienceYears, 0) AS MinYears,
       ISNULL(cc.IsPremium, 0) AS IsPremium,
       c.Price,
       cc.Name
FROM CarTbl c
LEFT JOIN CarCategory cc ON c.CategoryId = cc.CategoryId
WHERE c.CPlateNum = @plate", conn);
                cmd.Parameters.AddWithValue("@plate", carPlate);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        classMinYears = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        isPremium = !reader.IsDBNull(1) && reader.GetBoolean(1);
                        carPrice = reader.IsDBNull(2) ? (decimal?)null : Convert.ToDecimal(reader[2]);
                        currentCarClass = reader.IsDBNull(3) ? "Неизвестно" : reader.GetString(3);
                    }
                }

                // 1. FIRST check admin-assigned classes (Manual verification)
                var classCheck = new SqlCommand(@"
SELECT COUNT(*) FROM CustomerAllowedCategory 
WHERE CustomerId = @custId AND CategoryId = (SELECT CategoryId FROM CarTbl WHERE CPlateNum = @plate)", conn);
                classCheck.Parameters.AddWithValue("@custId", customerId);
                classCheck.Parameters.AddWithValue("@plate", carPlate);
                
                int count = (int)classCheck.ExecuteScalar();
                if (count > 0)
                {
                    // Admin explicitly allowed this class for this user.
                    return new EligibilityResult { Allowed = true };
                }

                // 2. If not explicitly allowed, check if user has ANY allowed classes
                var allowedClassesCmd = new SqlCommand(@"
                    SELECT cc.Name 
                    FROM CustomerAllowedCategory cac
                    JOIN CarCategory cc ON cac.CategoryId = cc.CategoryId
                    WHERE cac.CustomerId = @custId", conn);
                allowedClassesCmd.Parameters.AddWithValue("@custId", customerId);
                
                var allowedClassesList = new System.Collections.Generic.List<string>();
                using (var reader = allowedClassesCmd.ExecuteReader())
                {
                    while (reader.Read()) allowedClassesList.Add(reader.GetString(0));
                }

                if (allowedClassesList.Count > 0)
                {
                    string allowedText = string.Join(", ", allowedClassesList);
                    return new EligibilityResult
                    {
                        Allowed = false,
                        Reason = $"К сожалению, вам доступны автомобили только {allowedText} класса."
                    };
                }

                // 3. Automated check (based on experience) if no manual classes assigned yet
                int experienceYears = 0;
                DateTime? issueDate = null;

                var expCmd = new SqlCommand(@"
SELECT DrivingExperienceYears, LicenseIssueDate
FROM CustomerTbl
WHERE CustId = @custId", conn);
                expCmd.Parameters.AddWithValue("@custId", customerId);

                using (var reader = expCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) experienceYears = reader.GetInt32(0);
                        if (!reader.IsDBNull(1)) issueDate = reader.GetDateTime(1);
                    }
                }

                if (issueDate.HasValue)
                {
                    var years = (int)Math.Max(0, Math.Floor((DateTime.UtcNow - issueDate.Value).TotalDays / 365.25));
                    experienceYears = Math.Max(experienceYears, years);
                }

                if (experienceYears < classMinYears)
                {
                    return new EligibilityResult
                    {
                        Allowed = false,
                        Reason = $"Класс «{currentCarClass}» доступен водителям со стажем от {classMinYears} лет. Ваш стаж: {experienceYears}."
                    };
                }

                if (isPremium && experienceYears < 3)
                {
                    return new EligibilityResult
                    {
                        Allowed = false,
                        Reason = "Класс «Премиум» доступен только опытным водителям со стажем от 3 лет."
                    };
                }

                // 4. Fallback to documents status
                var docCheck = new SqlCommand("SELECT Status FROM DrivingLicense WHERE CustomerId = @custId ORDER BY CreatedAt DESC", conn);
                docCheck.Parameters.AddWithValue("@custId", customerId);
                string docStatus = docCheck.ExecuteScalar()?.ToString();

                if (docStatus == "Pending")
                {
                    return new EligibilityResult
                    {
                        Allowed = false,
                        Reason = "Ваши документы находятся на проверке у администратора. Пожалуйста, подождите."
                    };
                }
                else if (docStatus == "Approved")
                {
                     return new EligibilityResult
                    {
                        Allowed = false,
                        Reason = "Ваши документы одобрены, но администратор еще не назначил вам доступные классы автомобилей."
                    };
                }

                return new EligibilityResult
                {
                    Allowed = false,
                    Reason = "Для аренды необходимо загрузить водительское удостоверение в профиле и дождаться проверки."
                };
            }
        }
    }
}

