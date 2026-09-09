let productTable;

$(document).ready(function () {
    loadData();

    $('.categoryBtn').on('click', function () {
        const categoryId = $(this).data('categoryid');
        const url = '/api/products' + (categoryId ? `?categoryId=${categoryId}` : '');

        productTable.ajax.url(url).load();
    });
});

function loadData() {
    productTable = $('#productTable').DataTable({
        ajax: '/api/products',
        responsive: true,
        columns: [
            { data: 'id', width: "5%" },
            { data: 'name', width: "20%" },
            { data: 'description', width: "30%", className: 'd-none d-lg-table-cell' },
            {
                data: 'price', width: "15%",
                render: data => `$${parseFloat(data).toFixed(2)}`
            },
            {
                data: 'id', width: "20%",
                render: function (data) {
                    return `
                        <div class="d-flex justify-content-end gap-2">
                            <a href="/Product/Update/${data}" class="btn btn-primary btn-sm">
                                <i class="bi bi-pencil"></i> Edit
                            </a>
                            <button onclick="deleteProduct('/api/delete/${data}')" class="btn btn-danger btn-sm">
                                <i class="bi bi-trash"></i> Delete
                            </button>
                        </div>`;
                }
            }
        ],
        language: {
            emptyTable: "No products found"
        },
        order: [[0, 'desc']],
        pageLength: 10,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]]
    });
}

async function deleteProduct(url) {
    const result = await Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    });

    if (result.isConfirmed) {
        try {
            const response = await fetch(url, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' }
            });
            const data = await response.json();

            if (response.ok && data.success) {
                productTable.ajax.reload();
                Swal.fire("Deleted!", data.message, "success");
            } else {
                Swal.fire("Error!", data.message, "error");
            }
        } catch (err) {
            console.error(err);
            Swal.fire("Error!", "An error occurred while deleting the product.", "error");
        }
    }
}
