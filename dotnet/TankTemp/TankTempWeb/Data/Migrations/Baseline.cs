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
            Create.Table("Sensors")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("PartNumber").AsString(50).NotNullable().Unique()
                .WithColumn("Description").AsString(128).Nullable();

            Create.Table("TemperatureObservations")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Tempature").AsFloat().NotNullable()
                .WithColumn("ObservedAt").AsDateTime().NotNullable()
                .WithColumn("SensorId").AsInt32().ForeignKey("TemperatureObservations", "Sensors", "Id");

            Create.Table("HourlyTemperatureObservationSummaries")
                .WithColumn("Id").AsInt32().NotNullable().Identity()
                .WithColumn("HightTemperature").AsFloat().NotNullable()
                .WithColumn("LowTemperature").AsFloat().NotNullable()
                .WithColumn("AverageTemperature").AsFloat().NotNullable()
                .WithColumn("Period").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("HourlyTemperatureObservationSummaries");
            Delete.Table("TemperatureObservations");
            Delete.Table("Sensors");
        }
    }
}