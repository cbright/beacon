import select,socket
import json
import mysql.connector

UDP_PORT = 3968

sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
cnx = mysql.connector.connect(user='cbright',database='moisture_data',host='192.168.1.71')
INSERT_READING = ("INSERT INTO live (%(Id)s, %(VOLTAGE))")

try:
	try:
		sock.bind(('0.0.0.0',UDP_PORT))
	except:
		print('Failed to bind')
		raise

	while(1):
		print('listening...')
		data, server = sock.recvfrom(4096)
		print('data received from {}: {}').format(server,data)
		
		sensorReadings = json.loads(data)
		
		for sensor in sensorReadings:
			cursor = cnx.cursor()
			cursor.execute(INSERT_READING, sensor)
			cnx.commit()
			cursor.close() 
            
finally:
	sock.close()
	cnx.close()
	
