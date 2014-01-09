SELECT count(*) FROM moisture_data.experiment1a;

select  * from moisture_data.experiment1a order by created desc LIMIT 0,2 

select sensor, min(voltage),max(voltage) from moisture_data.experiment1a group by sensor