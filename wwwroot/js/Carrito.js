$(document).ready(() => {
    mostrarCarrito();
});


document.getElementById('tipoPago').addEventListener('change', function () {
    var tarjetaContainer = document.getElementById('tarjetaContainer');
    if (this.value === 'tarjeta') {
        tarjetaContainer.style.display = 'block';
    } else {
        tarjetaContainer.style.display = 'none';
    }
});




const mostrarCarrito = () => {
    let carrito = JSON.parse(localStorage.getItem('carrito')) || [];

    const carritoBody = document.getElementById('carrito-body');
    carritoBody.innerHTML = '';

    let subtotal = 0;

    carrito.forEach(function (item) {
        let precio = parseInt(item.precio);
        let total = precio * item.cantidad;
        let totalMostrar = total.toFixed(2).replace('.', ',');
        subtotal += total;

        let newRow = document.createElement('tr');
        newRow.innerHTML = `
            <td>
                <div class="d-flex align-items-center">
                    <img src="data:image;base64,${item.imagen}" class="img-fluid mr-3" style="max-width: 80px; height: auto; object-fit: cover;">
                    <div>
                        <h5 class="p-2 text-white">${item.nombre}</h5>
                    </div>
                </div>
            </td>
            <td class="align-middle text-white">₡ ${item.precio}</td>
            <td class="align-middle">
                <input type="number" class="form-control text-center" min="1" max="${item.stock}" value="${item.cantidad}" onchange="actualizarCantidad(${item.id}, this.value)">
            </td>
            <td class="align-middle text-white">₡ ${totalMostrar}</td>
            <td class="align-middle">
                <button class="btn btn-danger btn-sm" onclick="eliminarProducto(${item.id})">Eliminar</button>
            </td>
        `;
        carritoBody.appendChild(newRow);
    });

    document.getElementById('subtotal').textContent = `${subtotal}`;
   
    let iva = subtotal * 0.13;
    document.getElementById('iva').textContent = `${iva.toFixed(2)}`;
    document.getElementById('total').textContent = `${(subtotal + iva).toFixed(2)}`;

    const pagarButton = document.getElementById('pagarButton');
    pagarButton.style.display = carrito.length > 0 ? 'block' : 'none';
}

const eliminarProducto = (id) => {
    console.log('Eliminando producto con id:', id);
    let carrito = JSON.parse(localStorage.getItem('carrito')) || [];
    let nuevoCarrito = carrito.filter(item => item.id !== String(id));

    localStorage.setItem('carrito', JSON.stringify(nuevoCarrito));

    mostrarCarrito();
};


const actualizarCantidad = (id, nuevaCantidad) => {
    console.log('Actualizando cantidad del producto con id:', id, 'nueva cantidad:', nuevaCantidad);
    let carrito = JSON.parse(localStorage.getItem('carrito')) || [];
    carrito.forEach(item => {
        if (item.id === String(id)) {
            item.cantidad = parseInt(nuevaCantidad, 10);
        }
    });

    localStorage.setItem('carrito', JSON.stringify(carrito));

    mostrarCarrito();
}


const guardarCarrito = () => {
    
    const direccion = document.getElementById('direccion').value;
    const canton = document.getElementById('canton').value;
    const provincia = document.getElementById('provincia').value;
    const codigoPostal = document.getElementById('codigoPostal').value;
    const tipoPago = document.getElementById('tipoPago').value;
    const numeroTarjeta = document.getElementById('numeroTarjeta').value;

   
    if (!direccion || !canton || !provincia || !codigoPostal || !tipoPago || (tipoPago === 'tarjeta' && !numeroTarjeta)) {
        alert('Por favor, complete todos los campos obligatorios.');
        return;
    }





    const carrito = JSON.parse(localStorage.getItem('carrito')) || [];

    const carritoSinImagen = carrito.map(articulo => {
        return {
            ArticuloId: parseInt(articulo.id),
            Nombre: articulo.nombre,
            Precio: parseFloat(articulo.precio),
            Cantidad: articulo.cantidad
        };
    });

    console.log('Iniciando proceso de guardar carrito...');

    console.log(carritoSinImagen);

    $.ajax({
        url: '/Home/GuardarCarrito',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(carritoSinImagen),
        success: function (response) {             
            Swal.fire({
                title: "¡Éxito!",
                text: "Su compra se ha realizado puede ver la misma en 'Mis Compras'.",
                icon: "success",
                confirmButtonColor: "#3085d6"
            }).then((result) => {
                localStorage.clear();
                location.reload();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Swal.fire({
                title: "¡Error!",
                text: "Ha ocurrido un error al realizar la transacción",
                icon: "error",
                confirmButtonColor: "#3085d6"
            });
        }
    });
}



function validarFormulario() {
    const direccion = document.getElementById('direccion');
    const canton = document.getElementById('canton');
    const provincia = document.getElementById('provincia');
    const codigoPostal = document.getElementById('codigoPostal');
    const tipoPago = document.getElementById('tipoPago');
    const numeroTarjeta = document.getElementById('numeroTarjeta');

    let valid = true;

    // Reset validation messages
    resetValidationMessages();

    if (direccion.value.trim() === '') {
        mostrarError('direccion');
        valid = false;
    }
    if (canton.value.trim() === '') {
        mostrarError('canton');
        valid = false;
    }
    if (provincia.value.trim() === '') {
        mostrarError('provincia');
        valid = false;
    }
    if (codigoPostal.value.trim() === '') {
        mostrarError('codigoPostal');
        valid = false;
    }
    if (tipoPago.value === '') {
        mostrarError('tipoPago');
        valid = false;
    }
    if (tipoPago.value === 'tarjeta' && numeroTarjeta.value.trim() === '') {
        mostrarError('numeroTarjeta');
        valid = false;
    }

    if (valid) {
        guardarCarrito(); // Llama a tu función para guardar el carrito
    }
}

function mostrarError(campo) {
    const errorDiv = document.getElementById(`${campo}Error`);
    if (errorDiv) {
        errorDiv.style.display = 'block';
    }
}

function resetValidationMessages() {
    const errorDivs = document.querySelectorAll('.invalid-feedback');
    errorDivs.forEach(div => {
        div.style.display = 'none';
    });
}