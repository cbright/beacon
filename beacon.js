var connect = require('connect');
var app = require('express').createServer();
var io = require('socket.io').listen(app);

app.get('/',function(req,res){
	res.sendfile(__dirname + '/index.html');
});

app.get('/bus',function(req,res){
	res.sendfile(__dirname + '/bus.html');
});

app.post('/bus',function(req,res){
	io.sockets.emit('echo-crate',{message : req.body});
});

io.sockets.on('connection',function(socket) {
	socket.emit('echo-crate',{status:'in crate'});
});
	
app.use(connect.bodyParser());
	
app.listen(1337);