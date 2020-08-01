// var g = G$('CHRISSY', 'LIU');
// console.log(g);
// g.greet().setLang('en').greet(true);
// g.log().setLang('zh_cn').greet(true);

$('#login').on('click', 'button', function() {
    let pj = G$("CHRIS", "LIU");
    pj.setLang($("#Lang").val()).greetHTML('#logMsg', true);
})