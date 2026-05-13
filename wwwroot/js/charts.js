// Chart.js interop functions called from Dashboard.razor via IJSRuntime

const CHART_COLORS = [
    '#4f46e5', '#06b6d4', '#10b981', '#f59e0b',
    '#ef4444', '#8b5cf6', '#ec4899', '#14b8a6'
];

let categoryChartInstance = null;
let dailyChartInstance = null;

window.renderCategoryChart = (canvasId, labels, data) => {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    if (categoryChartInstance) categoryChartInstance.destroy();

    categoryChartInstance = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: CHART_COLORS,
                borderWidth: 2,
                borderColor: '#fff'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: { padding: 12, font: { size: 11 } }
                }
            }
        }
    });
};

window.renderDailyChart = (canvasId, labels, data) => {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    if (dailyChartInstance) dailyChartInstance.destroy();

    dailyChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Daily Spending ($)',
                data: data,
                backgroundColor: 'rgba(79, 70, 229, 0.7)',
                borderColor: '#4f46e5',
                borderWidth: 1,
                borderRadius: 4
            }]
        },
        options: {
            responsive: true,
            plugins: { legend: { display: false } },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: { color: 'rgba(0,0,0,0.05)' },
                    ticks: { callback: v => '$' + v }
                },
                x: { grid: { display: false } }
            }
        }
    });
};
