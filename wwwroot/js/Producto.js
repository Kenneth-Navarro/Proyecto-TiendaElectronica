
function changeMainImage(imageSrc) {
    const mainImage = document.getElementById("mainImage");
    mainImage.src = 'data:image;base64,' + imageSrc;
}

function agregarAlCarrito(id, nombre, precio, imagen, stock) {
    
    const cantidad = parseInt(document.getElementById("cantidad").value);
    const stockDisponible = parseInt(document.getElementById("stock").innerText);

    const carrito = JSON.parse(localStorage.getItem('carrito')) || [];

    let articuloExistente = carrito.find(item => item.id === id);
    let cantidadEnCarrito = articuloExistente ? articuloExistente.cantidad : 0;
    let cantidadDisponible = stockDisponible - cantidadEnCarrito;


    if (cantidadDisponible === 0) {
        Swal.fire({
            icon: 'warning',
            title: 'Sin stock',
            text: 'Este producto ya no está disponible para agregar al carrito.',
            confirmButtonText: 'Entendido',
            customClass: {
                confirmButton: 'my-custom-button'
            }
        });
        return;
    }

    if (cantidad > cantidadDisponible) {
        Swal.fire({
            icon: 'warning',
            title: 'Cantidad excedida',
            text: `Solo puedes agregar ${cantidadDisponible} más al carrito.`,
            confirmButtonText: 'Entendido',
            customClass: {
                confirmButton: 'my-custom-button'
            }
        });
        return;
    }

   
    var articulo = {
        id: id,
        nombre: nombre,
        precio: precio,
        cantidad: cantidad,
        imagen: imagen,
        stock: stockDisponible  
    };

    if (articuloExistente) {
        articuloExistente.cantidad += cantidad;
    } else {
        carrito.push(articulo);
    }

    localStorage.setItem('carrito', JSON.stringify(carrito));

    Swal.fire({
        icon: 'success',
        title: 'Artículo agregado',
        text: 'El artículo ha sido agregado al carrito.',
        confirmButtonText: 'OK',
        customClass: {
            confirmButton: 'my-custom-button'
        }
    }).then(() => {
        location.reload();
    });

    console.log('Carrito actualizado:', carrito);
}

function agregarProductoAlCarrito(id, nombre, precio, imagen, stock) {
    stock = parseInt(stock); 
    let carrito = JSON.parse(localStorage.getItem('carrito')) || [];

    
    let productoExistente = carrito.find(item => item.id === id);
    let cantidadEnCarrito = productoExistente ? productoExistente.cantidad : 0;
    let cantidadDisponible = stock - cantidadEnCarrito;

    if (cantidadDisponible <= 0) {
        Swal.fire({
            icon: 'error',
            title: 'Stock insuficiente',
            text: 'Ya no puedes agregar más de este artículo al carrito.',
            confirmButtonText: 'Entendido',
            customClass: {
                confirmButton: 'my-custom-button'
            }
        });
        return;
    }

    if (productoExistente) {
      
        productoExistente.cantidad += 1;
    } else {
        // Si el producto no existe, agregarlo al carrito
        let nuevoProducto = {
            id: id,
            nombre: nombre,
            precio: precio,
            imagen: imagen,
            cantidad: 1,
            stock: stock    
        };
        carrito.push(nuevoProducto);


    }
    Swal.fire({
        icon: 'success',
        title: 'Artículo agregado',
        text: 'El artículo ha sido agregado al carrito.',
        confirmButtonText: 'OK',
        customClass: {
            confirmButton: 'my-custom-button'
        }
    });

    localStorage.setItem('carrito', JSON.stringify(carrito));
}
