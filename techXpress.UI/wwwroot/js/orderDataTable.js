$(document).ready(function () {
    loadData();
});

function loadData() {
    $('#orderTable').DataTable({
        ajax: '/api/orders',
        responsive: true,
        columns: [
            { data: 'orderId', width: "10%" },
            { data: 'userEmail', width: "20%" },
            {
                data: 'totalAmount',
                width: "15%",
                render: function (data) {
                    return '$' + parseFloat(data).toFixed(2);
                }
            },
            {
                data: 'orderDate',
                width: "20%",
                render: function (data) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                data: 'orderStatus',
                width: "15%",
                render: function (data) {
                    let badgeClass = '';
                    switch (data) {
                        case 'Pending': badgeClass = 'bg-warning'; break;
                        case 'Approved': badgeClass = 'bg-info'; break;
                        case 'InProgress': badgeClass = 'bg-primary'; break;
                        case 'Shipped': badgeClass = 'bg-success'; break;
                        case 'Canceled': badgeClass = 'bg-danger'; break;
                        default: badgeClass = 'bg-secondary';
                    }
                    return `<span class="badge ${badgeClass}">${data}</span>`;
                }
            },
            {
                data: 'orderId',
                width: "20%",
                orderable: false,
                render: function (data) {
                    return `<div class="text-end">
                                <a href="/Order/Update/${data}" class="btn btn-primary btn-sm">
                                    <i class="bi bi-pencil"></i> Update
                                </a>
                            </div>`;
                }
            }
        ],
        language: {
            emptyTable: "No orders found"
        },
        order: [[0, 'desc']],
        pageLength: 10,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]]
    });
}