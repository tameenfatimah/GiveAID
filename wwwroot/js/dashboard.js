// show table
function showTable(name)
{
    document.getElementById('stats-section').style.display = 'none';
    document.getElementById('recent-donations').style.display = 'none';
    document.querySelectorAll('.manage-table').forEach(function (t)
    {
        t.style.display = 'none';
    });
    document.getElementById('manage-section').style.display = 'block';
    document.getElementById('table-' + name).style.display = 'block';
}
// hide stats
function showDashboard()
{
    document.getElementById('manage-section').style.display = 'none';
    document.getElementById('stats-section').style.display = 'block';
    document.getElementById('recent-donations').style.display = 'block';
}
// Auto open table from URL parameter
var urlParams = new URLSearchParams(window.location.search);
var showOnLoad = urlParams.get('show');
if (showOnLoad) {
    showTable(showOnLoad, 'link-' + showOnLoad);
}