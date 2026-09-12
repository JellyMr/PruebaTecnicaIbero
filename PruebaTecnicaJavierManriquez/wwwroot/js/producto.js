$(document).ready(function () {

    cargarProductos();

    $("#formProducto").on("submit", function (e) {

        e.preventDefault();

        if (!validarFormulario()) {
            return;
        }

        guardarProducto();
    });

});

function cargarProductos() {

    $.ajax({
        url: "/api/Producto",
        type: "GET",

        success: function (response) {

            if (response.success) {

                mostrarProductos(response.data);

            } else {

                mostrarError(response.message);
            }
        },

        error: function (xhr) {

            mostrarError(
                obtenerMensajeError(
                    xhr,
                    "Ocurrió un error al consultar los productos."
                )
            );
        }
    });
}

function mostrarProductos(productos) {

    let html = "";

    if (productos.length === 0) {

        html = `
            <tr>
                <td colspan="6" class="text-center text-muted">
                    No hay productos registrados.
                </td>
            </tr>
        `;

    } else {

        productos.forEach(function (producto) {

            html += `
                <tr>
                    <td>${producto.idProducto}</td>

                    <td>${producto.nombre}</td>

                    <td>${producto.descripcion ?? ""}</td>

                    <td>$${Number(producto.precio).toFixed(2)}</td>

                    <td>${producto.existencia}</td>

                    <td>
                        <button type="button"
                                class="btn btn-danger btn-sm"
                                onclick="eliminarProducto(${producto.idProducto})">
                            Eliminar
                        </button>
                    </td>
                </tr>
            `;

        });
    }

    $("#tablaProductos").html(html);
}

function validarFormulario() {

    let valido = true;

    limpiarErrores();

    const nombre = $("#nombre").val().trim();
    const precio = parseFloat($("#precio").val());
    const existencia = parseInt($("#existencia").val());

    if (nombre === "") {

        $("#nombre").addClass("is-invalid");

        valido = false;
    }

    if (isNaN(precio) || precio <= 0) {

        $("#precio").addClass("is-invalid");

        valido = false;
    }

    if (isNaN(existencia) || existencia < 0) {

        $("#existencia").addClass("is-invalid");

        valido = false;
    }

    return valido;
}

function guardarProducto() {

    const producto = {

        nombre: $("#nombre").val().trim(),
        descripcion: $("#descripcion").val().trim(),
        precio: parseFloat($("#precio").val()),
        existencia: parseInt($("#existencia").val())
    };

    $("#btnGuardar").prop("disabled", true);

    $.ajax({

        url: "/api/Producto",

        type: "POST",

        contentType: "application/json",

        data: JSON.stringify(producto),

        success: function (response) {

            if (response.success) {

                mostrarExito(response.message);

                limpiarFormulario();

                cargarProductos();
            }
            else {

                mostrarError(response.message);
            }
        },

        error: function (xhr) {

            mostrarError(
                obtenerMensajeError(
                    xhr,
                    "Ocurrió un error al guardar el producto."
                )
            );
        },

        complete: function () {

            $("#btnGuardar").prop("disabled", false);
        }
    });
}

function eliminarProducto(id) {

    if (!confirm("¿Deseas dar de baja este producto?")) {
        return;
    }

    $.ajax({

        url: "/api/Producto/" + id,

        type: "DELETE",

        success: function (response) {

            if (response.success) {

                mostrarExito(response.message);

                cargarProductos();

            } else {

                mostrarError(response.message);
            }
        },

        error: function (xhr) {

            mostrarError(
                obtenerMensajeError(
                    xhr,
                    "Ocurrió un error al eliminar el producto."
                )
            );
        }
    });
}

function mostrarExito(mensaje) {

    $("#mensaje").html(`
        <div class="alert alert-success alert-dismissible fade show"
             role="alert">

            ${mensaje}

            <button type="button"
                    class="btn-close"
                    data-bs-dismiss="alert">
            </button>

        </div>
    `);
}

function mostrarError(mensaje) {

    $("#mensaje").html(`
        <div class="alert alert-danger alert-dismissible fade show"
             role="alert">

            ${mensaje}

            <button type="button"
                    class="btn-close"
                    data-bs-dismiss="alert">
            </button>

        </div>
    `);
}

function obtenerMensajeError(xhr, mensajeDefault) {

    if (xhr.responseJSON) {

        if (xhr.responseJSON.message) {
            return xhr.responseJSON.message;
        }

        if (xhr.responseJSON.errors) {

            let errores = [];

            Object.values(xhr.responseJSON.errors)
                .forEach(function (error) {

                    errores = errores.concat(error);

                });

            if (errores.length > 0) {
                return errores.join("<br>");
            }
        }
    }

    return mensajeDefault;
}

function limpiarErrores() {

    $("#nombre").removeClass("is-invalid");
    $("#precio").removeClass("is-invalid");
    $("#existencia").removeClass("is-invalid");
}

function limpiarFormulario() {

    $("#nombre").val("");
    $("#descripcion").val("");
    $("#precio").val("");
    $("#existencia").val("");

    limpiarErrores();
}