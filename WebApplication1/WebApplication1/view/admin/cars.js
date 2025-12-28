function updateSelectedImagesPreview() {
    const selectedImages = [];
    const mainImage = document.querySelector('input[id*="rbMain"]:checked');
    const galleryImages = document.querySelectorAll('input[id*="chkAttach"]:checked');
    
    console.log('Updating preview - MainImage:', mainImage ? 'found' : 'none', 'Gallery:', galleryImages.length);
    
    // Сначала добавляем главное изображение
    if (mainImage) {
        const imageItem = mainImage.closest('.image-item');
        if (imageItem) {
            const img = imageItem.querySelector('img');
            const fileId = imageItem.querySelector('input[id*="hfFileId"]').value;
            console.log('Main image found:', img.src, 'ID:', fileId);
            selectedImages.push({
                src: img.src,
                id: fileId,
                type: 'main'
            });
        }
    }
    
    // Затем добавляем галерею, исключая главное изображение
    galleryImages.forEach(checkbox => {
        const imageItem = checkbox.closest('.image-item');
        if (imageItem) {
            const img = imageItem.querySelector('img');
            const fileId = imageItem.querySelector('input[id*="hfFileId"]').value;
            
            // Проверяем, что это изображение не выбрано как главное
            const isMainImage = mainImage && mainImage.closest('.image-item').querySelector('input[id*="hfFileId"]').value === fileId;
            
            if (!isMainImage) {
                console.log('Gallery image added:', img.src, 'ID:', fileId);
                selectedImages.push({
                    src: img.src,
                    id: fileId,
                    type: 'gallery'
                });
            }
        }
    });
    
    console.log('Total selected images:', selectedImages.length);
    
    const previewContainer = document.getElementById('selectedImagesPreview');
    const gridContainer = document.getElementById('selectedImagesGrid');
    
    if (selectedImages.length > 0) {
        previewContainer.style.display = 'block';
        gridContainer.innerHTML = selectedImages.map((img, index) => `
            <div class="selected-image-item ${img.type === 'main' ? 'main-image' : ''}">
                <img src="${img.src}" alt="Selected image ${index + 1}" />
                ${img.type === 'main' ? '<div class="main-badge">Главное</div>' : ''}
                <button type="button" class="remove-btn" onclick="removeSelectedImage('${img.id}', '${img.type}')">×</button>
            </div>
        `).join('');
    } else {
        previewContainer.style.display = 'none';
        gridContainer.innerHTML = '';
    }
}

function removeSelectedImage(fileId, type) {
    const modal = document.getElementById('serverImagesModal');
    const imageItems = modal.querySelectorAll('.image-item');
    
    imageItems.forEach(item => {
        const hiddenField = item.querySelector('input[id*="hfFileId"]');
        if (hiddenField && hiddenField.value === fileId) {
            if (type === 'main') {
                const radio = item.querySelector('input[id*="rbMain"]');
                if (radio) {
                    radio.checked = false;
                    radio.dataset.wasChecked = 'false';
                }
            } else {
                const checkbox = item.querySelector('input[id*="chkAttach"]');
                if (checkbox) {
                    checkbox.checked = false;
                    checkbox.dataset.wasChecked = 'false';
                }
            }
        }
    });
    
    updateSelectedImagesPreview();
}

function clearSelectedImages() {
    const modal = document.getElementById('serverImagesModal');
    const allRadios = modal.querySelectorAll('input[id*="rbMain"]');
    const allCheckboxes = modal.querySelectorAll('input[id*="chkAttach"]');
    
    allRadios.forEach(radio => {
        radio.checked = false;
        radio.dataset.wasChecked = 'false';
    });
    allCheckboxes.forEach(checkbox => {
        checkbox.checked = false;
        checkbox.dataset.wasChecked = 'false';
    });
    
    updateSelectedImagesPreview();
}

document.addEventListener('DOMContentLoaded', function() {
    const modal = document.getElementById('serverImagesModal');
    
    modal.addEventListener('change', function(e) {
        if (e.target.matches('input[id*="rbMain"]')) {
            // Если выбрана главная картинка, снимаем выбор с других главных
            const allRadios = modal.querySelectorAll('input[id*="rbMain"]');
            allRadios.forEach(radio => {
                if (radio !== e.target) {
                    radio.checked = false;
                    radio.dataset.wasChecked = 'false';
                }
            });
            updateSelectedImagesPreview();
        } else if (e.target.matches('input[id*="chkAttach"]')) {
            updateSelectedImagesPreview();
        }
    });
    
    // Добавляем обработчик для отмены выбора radio кнопки
    document.addEventListener('click', function(e) {
        if (e.target.matches('input[id*="rbMain"]')) {
            const radio = e.target;
            if (radio.dataset.wasChecked === 'true') {
                // Если кнопка была выбрана, отменяем выбор
                setTimeout(() => {
                    radio.checked = false;
                    radio.dataset.wasChecked = 'false';
                    updateSelectedImagesPreview();
                }, 0);
            } else {
                // Отмечаем что кнопка теперь выбрана
                radio.dataset.wasChecked = 'true';
                // Принудительно обновляем превью после выбора
                setTimeout(() => {
                    updateSelectedImagesPreview();
                }, 10);
            }
        } else if (e.target.matches('input[id*="chkAttach"]')) {
            const checkbox = e.target;
            if (checkbox.dataset.wasChecked === 'true') {
                // Если чекбокс был выбран, отменяем выбор
                setTimeout(() => {
                    checkbox.checked = false;
                    checkbox.dataset.wasChecked = 'false';
                    updateSelectedImagesPreview();
                }, 0);
            } else {
                // Отмечаем что чекбокс теперь выбран
                checkbox.dataset.wasChecked = 'true';
                // Принудительно обновляем превью после выбора
                setTimeout(() => {
                    updateSelectedImagesPreview();
                }, 10);
            }
        }
    });
    
    modal.addEventListener('hidden.bs.modal', function() {
        updateSelectedImagesPreview();
    });
});
