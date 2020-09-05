(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = 'https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.2&appId=2210055362615403&autoLogAppEvents=1';
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
var data = [1, 2, 3, 4];
var w = 500;
var h = 100;

var svg = d3.select("#barChart")
    .attr("width", w)
    .attr("height", h);

svg.selectAll("rect")
    .data(data)
    .enter()
    .append("rect")
    .attr("x", function (d, i) {
        return i * 21;  //Bar width of 20 plus 1 for padding
    })
    .attr("y", function (d) {
        return h - d;  //Height minus data value
    })
    .attr("width", 20)
    .attr("height", function (d) {
        return d;  //Just the data value
    })
    .attr("fill", function (d) {
        return "rgb(0, 0, " + (d * 3) + ")";
    });