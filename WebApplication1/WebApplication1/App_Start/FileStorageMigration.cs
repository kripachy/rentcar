using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.App_Start
{
    /// <summary>
    /// Migrates old absolute file paths to relative paths for project portability.
    /// Should be called once when deploying to a new server/computer.
    /// </summary>
    public static class FileStorageMigration
    {
        /// <summary>
        /// Migrates all absolute paths in FileStorage to relative paths.
        /// Copies files from old absolute paths to ~/uploads/ if they exist.
        /// Safe to call multiple times - only processes absolute paths.
        /// </summary>
        public static void MigrateToRelativePaths(HttpServerUtility server)
        {
            string cs = Functions.GetConnectionString();
            var storage = new FileStorageService(server: server);
            string uploadsRoot = server.MapPath("~/uploads");

            // First, collect all files that need migration
            var filesToMigrate = new List<(int fileId, string oldPath, string fileKind, string originalName)>();
            
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
SELECT FileId, FilePath, FileKind, OriginalName
FROM FileStorage", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int fileId = reader.GetInt32(0);
                        string storedPath = reader.GetString(1);
                        string fileKind = reader.GetString(2);
                        string originalName = reader.GetString(3);

                        // Skip if already relative path (starts with "uploads/")
                        if (storedPath.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase) ||
                            storedPath.StartsWith("uploads\\", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        // Check if path is absolute (contains drive letter or network path)
                        if (Path.IsPathRooted(storedPath))
                        {
                            filesToMigrate.Add((fileId, storedPath, fileKind, originalName));
                        }
                    }
                }
            }

            // Now migrate each file (using separate connection for updates)
            string siteRoot = server.MapPath("~/");
            
            foreach (var file in filesToMigrate)
            {
                try
                {
                    // Check if old file exists
                    if (!File.Exists(file.oldPath))
                    {
                        // File doesn't exist - skip migration (might be already deleted)
                        continue;
                    }

                    // Determine new relative folder
                    string relativeFolder = file.fileKind ?? "misc";
                    string extension = Path.GetExtension(file.originalName);
                    if (string.IsNullOrEmpty(extension))
                    {
                        extension = Path.GetExtension(file.oldPath);
                    }

                    string newFileName = $"{Guid.NewGuid():N}{extension}";
                    string newFullPath = Path.Combine(uploadsRoot, relativeFolder, newFileName);
                    string newFolder = Path.GetDirectoryName(newFullPath);

                    // Create directory if needed
                    if (!Directory.Exists(newFolder))
                    {
                        Directory.CreateDirectory(newFolder);
                    }

                    // Copy file to new location
                    File.Copy(file.oldPath, newFullPath, overwrite: false);

                    // Calculate relative path
                    string relativePath = newFullPath.Substring(siteRoot.Length).Replace('\\', '/');

                    // Update database with new relative path
                    using (var conn = new SqlConnection(cs))
                    {
                        conn.Open();
                        var updateCmd = new SqlCommand(@"
UPDATE FileStorage 
SET FilePath = @newPath 
WHERE FileId = @fileId", conn);
                        updateCmd.Parameters.AddWithValue("@newPath", relativePath);
                        updateCmd.Parameters.AddWithValue("@fileId", file.fileId);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue with other files
                    System.Diagnostics.Debug.WriteLine($"Error migrating file {file.fileId}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// One-time migration that should be called on application start.
        /// Checks if migration is needed and performs it if necessary.
        /// </summary>
        public static void EnsureMigrated(HttpServerUtility server)
        {
            // Check if there are any absolute paths that need migration
            string cs = Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                // Check if migration is needed by checking for absolute paths
                var checkCmd = new SqlCommand(@"
SELECT FilePath
FROM FileStorage
WHERE FilePath NOT LIKE 'uploads/%' AND FilePath NOT LIKE 'uploads\\%'", conn);

                bool needsMigration = false;
                using (var reader = checkCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string path = reader.GetString(0);
                        if (Path.IsPathRooted(path))
                        {
                            needsMigration = true;
                            break;
                        }
                    }
                }

                if (needsMigration)
                {
                    // Migrate absolute paths to relative
                    MigrateToRelativePaths(server);
                }
            }
        }
    }
}

