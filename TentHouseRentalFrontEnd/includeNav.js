$(document).ready(function() {  
    $('#navbar').load('/navbar.html');
    
    var jwtToken = (localStorage.getItem('jwtToken') || null);
    if(jwtToken == null){
        window.location.href = '/Login/login.html';
    } 

    $("#logOutBtn").click(logOut);
});

function logOut(){
    if (localStorage.getItem("jwtToken")) {
        localStorage.clear(); 
    }
    window.location.href ="/Login/login.html";
}

