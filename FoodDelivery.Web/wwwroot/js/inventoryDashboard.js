document.addEventListener("DOMContentLoaded", () => {
    const branchFilter = $('#branchFilter');
    const warehouseFilter = $('#warehouseFilter');

    // Load branch and warehouse filters
    function loadBranchAndWarehouseFilters() {
        $.get('/Admin/Branch/GetAllBranchesWithWarehouses', function (data) {
            branchFilter.empty();
            warehouseFilter.empty();

            branchFilter.append('<option value="all">All Branches</option>');
            warehouseFilter.append('<option value="all">All Warehouses</option>');

            data.forEach(branch => {
                branchFilter.append(`<option value="${branch.branchId}">${branch.branchName}</option>`);
            });

            branchFilter.change(function () {
                const selectedBranchId = $(this).val();
                warehouseFilter.empty();
                warehouseFilter.append('<option value="all">All Warehouses</option>');

                if (selectedBranchId === "all") {
                    updateDashboardData(null, null);
                    fetchAndRenderCharts(null, null);
                    return;
                }

                const selectedBranch = data.find(branch => branch.branchId == selectedBranchId);
                if (selectedBranch && selectedBranch.warehouses) {
                    selectedBranch.warehouses.forEach(warehouse => {
                        warehouseFilter.append(`<option value="${warehouse.warehouseId}">${warehouse.warehouseName}</option>`);
                    });
                }

                updateDashboardData(selectedBranchId, null);
                fetchAndRenderCharts(selectedBranchId, null);
            });

            warehouseFilter.change(function () {
                const selectedBranchId = branchFilter.val();
                const selectedWarehouseId = $(this).val();

                if (selectedWarehouseId === "all") {
                    updateDashboardData(selectedBranchId, null);
                    fetchAndRenderCharts(selectedBranchId, null);
                } else {
                    updateDashboardData(selectedBranchId, selectedWarehouseId);
                    fetchAndRenderCharts(selectedBranchId, selectedWarehouseId);
                }
            });

            updateDashboardData(null, null);
            fetchAndRenderCharts(null, null);
        });
    }

    // Fetch and update dashboard KPIs
    function updateDashboardData(branchId, warehouseId) {
        const branchParam = branchId === "all" ? "" : `branchId=${branchId}`;
        const warehouseParam = warehouseId === "all" ? "" : `&warehouseId=${warehouseId}`;

        $.get(`/Admin/InventoryDashboard/GetDashboardData?${branchParam}${warehouseParam}`, function (data) {
            $('#stockValue').text(data.stockValue ?? 'N/A');
            $('#stockQuantity').text(data.stockQuantity ?? 'N/A');
            $('#reorderAlerts').text(data.reorderAlerts ?? 'N/A');
            $('#expiringSoon').text(data.expiringSoon ?? 'N/A');

            renderAlertsTables(data);
        });
    }

    // Fetch and render charts
    function fetchAndRenderCharts(branchId, warehouseId) {
        const branchParam = branchId === "all" ? "" : `branchId=${branchId}`;
        const warehouseParam = warehouseId === "all" ? "" : `&warehouseId=${warehouseId}`;

        $.get(`/Admin/InventoryDashboard/GetChartData?${branchParam}${warehouseParam}`, function (data) {
            updateChartData('stockDistributionChart', data.stockDistribution, 'category', 'quantity');
            updateChartData('warehouseComparisonChart', data.warehouseComparison, 'warehouse', 'quantity');
            updateChartData('reorderLevelItemsChart', data.reorderLevelItems, 'product', 'quantity');
            updateChartData('stockValueTrendChart', data.stockValueTrend, 'date', 'value', 'line');
        });
    }

    // Update chart data without toggles
    function updateChartData(chartId, newData, labelKey, valueKey, chartType = 'bar') {
        const ctx = document.getElementById(chartId).getContext('2d');
        let chart = Chart.getChart(ctx.canvas);

        if (!chart) {
            chart = new Chart(ctx, {
                type: chartType,
                data: {
                    labels: [],
                    datasets: [{
                        data: [],
                        backgroundColor: ['#ff6384', '#36a2eb', '#ffcd56', '#4bc0c0'],
                    }]
                },
            });
        }

        chart.data.labels = newData.map(item => item[labelKey]);
        chart.data.datasets[0].data = newData.map(item => item[valueKey]);
        chart.update();
    }

    function renderAlertsTables(data) {
        const lowStockTable = $('#lowStockTable tbody').empty();
        data.lowStockAlerts.forEach(alert => {
            lowStockTable.append(`<tr>
                <td>${alert.productName}</td>
                <td>${alert.branchName}</td>
                <td>${alert.warehouseName}</td>
                <td>${alert.totalQuantity}</td>
                <td>${alert.reorderLevel}</td>
            </tr>`);
        });

        const expiringBatchTable = $('#expiringBatchTable tbody').empty();
        data.expiringBatchAlerts.forEach(alert => {
            expiringBatchTable.append(`<tr>
                <td>${alert.productName}</td>
                <td>${alert.batchNumber}</td>
                <td>${alert.branchName}</td>
                <td>${alert.warehouseName}</td>
                <td>${alert.expirationDate}</td>
                <td>${alert.quantity}</td>
            </tr>`);
        });
    }

    loadBranchAndWarehouseFilters();
});
