

function previewImage(input, previewClass) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $(previewClass).attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}


function displayImagePreview(event, previewId) {
    const input = event.target;
    const file = input.files[0];
    const preview = document.getElementById(previewId);

    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = 'block';
        }
        reader.readAsDataURL(file);
    }
}

function setupImagePreviewEvents() {
    document.getElementById('Imagen1').addEventListener('change', function (event) {
        displayImagePreview(event, 'preview1');
    });

    document.getElementById('Imagen2').addEventListener('change', function (event) {
        displayImagePreview(event, 'preview2');
    });

    document.getElementById('Imagen3').addEventListener('change', function (event) {
        displayImagePreview(event, 'preview3');
    });
}


document.addEventListener('DOMContentLoaded', setupImagePreviewEvents);
