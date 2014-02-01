using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline
{
    public class Perils
    {
    //    [perilId] [int] NOT NULL,
        public int PerilID { get; set; }
    //[lineId] [int] NULL,
        public int LineID { get; set; }
    //[subLineId] [int] NULL,
        public int SubLineID { get; set; }
    //[perilSName] [varchar](10) NULL,
        public string PerilSName { get; set; }
    //[perilName] [varchar](100) NULL,
        public string PerilName { get; set; }
    //[perilLName] [varchar](200) NULL,
        public string PerilLName { get; set; }
    //[perilType] [char](1) NULL,
        public string PerilType { get; set; }
    //[isActive] [bit] NULL,
        public bool IsActive { get; set; }
    //[perilCode] [int] NULL,
        public int PerilCode { get; set; }
    //[RI_COMM_RT] [money] NULL,
        public decimal RI_COMM_RT { get; set; }
    //[isLimitFixed] [bit] NULL,
        public bool IsLimitFixed { get; set; }
    //[defaultLimit] [varchar](30) NULL,
        public string DefaultLimit { get; set; }
    //[requiresLTOInterconn] [bit] NULL,
        public bool RequiresLTOInterconn { get; set; }
    //[requiresDSTonCOC] [bit] NULL,
        public bool RequiresDSTonCOC { get; set; }
    //[limitSI] [int] NULL,
        public int LimitSI { get; set; }
    //[rate] [float] NULL,
        public double Rate { get; set; }
    //[premium] [int] NULL,
        public int Premium { get; set; }
    //[policyRate] [float] NULL,
        public double PolicyRate { get; set; }
    //[policyPremium] [int] NULL,
        public decimal PolicyPremium { get; set; }
    //[limit] [int] NULL,
        public int Limit { get; set; }
    //[PC] [int] NULL,
        public int PC { get; set; }
    //[CVLightMedium] [int] NULL,
        public int CVLightMedium { get; set; }
    //[CVHeavy] [int] NULL,
        public int CVHeavy { get; set; }
    }
}