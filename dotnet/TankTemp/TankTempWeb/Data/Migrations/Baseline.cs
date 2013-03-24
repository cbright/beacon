using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;
using FluentMigrator.Infrastructure;

namespace TankTempWeb.Data.Migrations
{
    [Migration(1)]
    public class Baseline : Migration
    {
        
        public override void Up()
        {
            Create.Table("Networks")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable();

            Create.Table("Sensors")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Network_Id").AsInt32().NotNullable().ForeignKey("FK_Network","Networks","Id")
                .WithColumn("discriminator").AsString(255).NotNullable()
                .WithColumn("SerialNumber").AsString(255).NotNullable()
                .WithColumn("Description").AsString(255).Nullable()
                .WithColumn("Unit").AsString(255).NotNullable();

            Create.Table("TemperatureObservations")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Value").AsFloat().NotNullable()
                .WithColumn("ObservedAt").AsDateTime().NotNullable()
                .WithColumn("Sensor_Id").AsInt32().ForeignKey("FK_Sensors_TemperatureObservation", "Sensors", "Id").Indexed("IX_Sensors_TemperatureObservation");

            Create.Table("HourlySummaries")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Low").AsFloat().NotNullable()
                .WithColumn("High").AsFloat().NotNullable()
                .WithColumn("Median").AsFloat().NotNullable()
                .WithColumn("Mode").AsFloat().NotNullable()
                .WithColumn("StandardDeviation").AsFloat().NotNullable()
                .WithColumn("SensorId").AsInt32().NotNullable()
                .WithColumn("HourOf").AsDateTime().NotNullable();

            Create.Index("UX_SensorsInNetwork")
                .OnTable("Sensors")
                .OnColumn("Network_Id").Unique()
                .OnColumn("SerialNumber").Unique();

        }

        public override void Down()
        {
            Delete.Table("Networks");
            Delete.Table("Sensors");
            Delete.Table("TemperatureObservations");
            Delete.Table("HourlySummaries");
        }
    }
}