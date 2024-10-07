$(document).ready(() => {

    new DataTable('#table_id', {
        // Opciones adicionales
        "paging": true, // Habilitar paginación
        "ordering": true, // Habilitar ordenamiento por columnas
        "searching": true, // Habilitar búsqueda
        // Otros opciones según tus necesidades
    });

});


const eliminar = (id, URL) => {

    const esCedula = /^[0-9]{9}$/.test(id);

    let mensajeConfirmacion = "";
    if (esCedula) {
        mensajeConfirmacion = `Se eliminará el usuario con la cédula ${id}. ¡No se podrán deshacer los cambios!`;
    } else {
        mensajeConfirmacion = `Se eliminará el artículo #${id}. ¡No se podrán deshacer los cambios!`;
    }

    Swal.fire({
        title: "¿Está seguro?",
        html: mensajeConfirmacion,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        cancelButtonText: "Cancelar",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: URL + `?id=${id}`,
                type: "GET"
            }).done((response) => {
                if (response.success) {
                    Swal.fire({
                        title: "¡Hecho!",
                        text: response.message,
                        icon: "success"
                    }).then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        text: response.message,
                        icon: "error"
                    });
                }
            }).fail(() => {
                Swal.fire({
                    title: "Error",
                    text: "Ha ocurrido un error en el proceso",
                    icon: "error"
                });
            });
        }
    });
}
