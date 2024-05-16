$(document).ready(function () {
    // Cargar la lista de empleados al iniciar la vista
    loadEmployeeList();

    // Manejar el clic en el botón de búsqueda
    $('#searchButton').click(function () {
        var employeeId = $('#employeeId').val();
        if (employeeId) {
            searchEmployee(employeeId);
        } else {
            loadEmployeeList(); // Si el ID está vacío, cargar la lista completa de empleados
        }
    });
});

// Función para cargar la lista completa de empleados
function loadEmployeeList() {
    $.ajax({
        url: 'https://localhost:7150/api/employee/',
        type: 'GET',
        success: function (data) {
            // Obtener una lista de promesas para calcular el salario anual de cada empleado
            /*var promises = data.map(employee => calculateAnnualSalary(employee.id));

            // Esperar a que todas las promesas se resuelvan
            Promise.all(promises)
                .then(annualSalaries => {
                    // Asignar el salario anual a cada empleado
                    data.forEach((employee, index) => {
                        employee.annual_salary = annualSalaries[index];
                    });

                    // Mostrar la información del empleado
                    displayEmployeeInfo(data);
                })
                .catch(error => {
                    console.error('Error al calcular los salarios anuales:', error);
                    $('#employeeInfo').html('Error al calcular los salarios anuales.');
                });
                */
            displayEmployeeInfo(data);
        },
        error: function () {
            $('#employeeInfo').html('Error al recuperar la lista de empleados.');
        }
    });
}

// Función para buscar un empleado por su ID
function searchEmployee(employeeId) {
    var url = 'https://localhost:7150/api/employee/' + employeeId;
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {

            displayEmployeeInfo(data);
        },
        error: function () {
            $('#employeeInfo').html('Empleado no encontrado.');
        }
    });
}

// Función para mostrar la información del empleado en la página
function displayEmployeeInfo(data) {
    // Limpiar el contenido anterior
    $('#employeeInfo').empty();

    // Verificar si hay datos de empleado para mostrar

    if (data) {
        // Definir la cantidad de elementos por página

        var itemsPerPage = 8;
        var totalPages = 0;
        if (data.length > 1) {

            totalPages = Math.ceil(data.length / itemsPerPage);
        } else {
            data = convertToArray(data);
            totalPages = 1;
        }
        
        console.log("totalPages", totalPages)
        // Crear una tabla con formato utilizando Bootstrap
        var table = $('<table>').addClass('table table-striped');
        var thead = $('<thead>').append(
            $('<tr>').append(
                $('<th>').text('ID'),
                $('<th>').text('Nombre'),
                $('<th>').text('Salario'),
                $('<th>').text('Salario Anual'),
                $('<th>').text('Edad')
            )
        );
        var tbody = $('<tbody>');

        // Iterar sobre los empleados para mostrar solo los elementos de la página actual
        var startIndex = 0;
        var endIndex = Math.min(itemsPerPage, data.length);
        for (var i = startIndex; i < endIndex; i++) {
            var employee = data[i];
            var row = $('<tr>').append(
                $('<td>').text(employee.id),
                $('<td>').text(employee.employee_name),
                $('<td>').text(employee.employee_salary),
                //$('<td>').text(employee.annual_salary),
                $('<td>').text(employee.employee_salary*12),
                $('<td>').text(employee.employee_age)
            );
            tbody.append(row);
        }

        // Agregar encabezado y cuerpo a la tabla
        table.append(thead, tbody);

        // Agregar la tabla al elemento con el id 'employeeInfo'
        $('#employeeInfo').append(table);

        // Agregar paginación
        var pagination = $('<ul>').addClass('pagination justify-content-center');
        for (var page = 1; page <= totalPages; page++) {
            var listItem = $('<li>').addClass('page-item');
            var link = $('<a>').addClass('page-link').attr('href', '#').text(page);
            if (page === 1) {
                listItem.addClass('active');
            }
            listItem.append(link);
            pagination.append(listItem);
        }
        $('#employeeInfo').append(pagination);

        // Manejar el clic en los enlaces de paginación
        $('.pagination .page-link').click(function (e) {
            e.preventDefault();
            $('.pagination .page-item').removeClass('active');
            $(this).parent().addClass('active');

            var pageNum = parseInt($(this).text());
            startIndex = (pageNum - 1) * itemsPerPage;
            endIndex = Math.min(startIndex + itemsPerPage, data.length);
            tbody.empty();
            for (var i = startIndex; i < endIndex; i++) {
                var employee = data[i];
                var row = $('<tr>').append(
                    $('<td>').text(employee.id),
                    $('<td>').text(employee.employee_name),
                    $('<td>').text(employee.employee_salary),
                    $('<td>').text(employee.employee_salary * 12),
                    $('<td>').text(employee.employee_age)
                );
                tbody.append(row);
            }
        });
    } else {
        // Mostrar un mensaje si no hay datos de empleado
        $('#employeeInfo').html('<div class="alert alert-warning" role="alert">No se encontraron empleados.</div>');
    }
}

function convertToArray(data) {
    if (!Array.isArray(data)) {
        // Si data no es un array, lo envolvemos en un array
        return [data];
    } else {
        // Si data ya es un array, lo devolvemos sin cambios
        return data;
    }
}

// Función para calcular el salario anual de un empleado utilizando la lógica de negocio
function calculateAnnualSalary(employeeId) {


    return new Promise((resolve, reject) => {
        // Realizar una llamada a la lógica de negocio para calcular el salario anual
        $.ajax({
            url: '/api/EmployeeBusinessLogic/' + employeeId + '/annual-salary',
            type: 'GET',
            success: function (response) {
                resolve(response);
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}
