var express = require('express');
var app = express.createServer(express.logger()
	,express.bodyParser());
var io = require('socket.io').listen(app);

app.get('/',function(req,res){
	res.sendfile(__dirname + '/index.html');
});

app.get('/bus',function(req,res){
	res.sendfile(__dirname + '/bus.html');
});

app.post('/bus',function(req,res){
	console.log(req.body);
	io.sockets.emit(req.body.channel,{message:req.body.message});
	res.redirect('/');
});

io.sockets.on('connection',function(socket) {
})
	
app.listen(1337);