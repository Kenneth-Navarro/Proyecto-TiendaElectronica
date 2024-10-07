$(document).ready(() => {
    $.ajax({
        url: "/Admin/Graficos",
        type: "GET"
    }).done((response) => {
        if (response.success) {
            var labels = response.productos.map((producto) => { return producto.nombre; });
            var values = response.productos.map((producto) => { return producto.cantidadTotal; });

            var ctx = document.getElementById('myChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Productos más Vendidos',
                        data: values,
                        backgroundColor: ' #262626',
                        borderColor: '#E14848',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

            var labels = response.categorias.map((categoria) => { return categoria.nombreCategoria; });
            var values = response.categorias.map((categoria) => { return categoria.cantidadTotal; });

            var ctx = document.getElementById('myChart2').getContext('2d');
            var myChart2 = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Categoría más Vendida',
                        data: values,
                        backgroundColor: ' #262626',
                        borderColor: '#E14848',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });


            // Función para obtener el nombre del mes en formato abreviado
            function getMonthName(dateStr) {
                const date = new Date(dateStr);
                const options = { month: 'short' }; // 'short' para abreviado, 'long' para completo
                return new Intl.DateTimeFormat('es-ES', options).format(date); // Cambia 'es-ES' a tu localización si es necesario
            }

            // Obtener etiquetas (solo meses)
            var labels = response.ventas.map((venta) => {
                return getMonthName(venta.fecha);
            });
            var values = response.ventas.map((venta) => { return venta.totalVentas; });

            var ctx = document.getElementById('myChart3').getContext('2d');
            var myChart3 = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Ventas por Mes',
                        data: values,
                        fill: false, // No rellenar el área bajo la línea
                        borderColor: '#E14848',
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Mes'
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Ventas'
                            },
                            beginAtZero: true
                        }
                    }
                }
            });


        } else {
            Swal.fire({
                title: "Error",
                text: "Ha ocurrido un error en el proceso",
                icon: "error"
            });
            
        }
    }).fail(() => {
        Swal.fire({
            title: "Error",
            text: response.message,
            icon: "error"
        });
    });
});

