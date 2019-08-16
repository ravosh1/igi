using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace costoracle2.Models
{
    public class ProviderModel
    {
        public string ServiceProviderId { get; set; }
        public string ServiceProviderprice { get; set; }
     //   public string ServiceProvider { get; set; }
        public string vehiclevaluegroupid { get; set; }
        public string Range { get; set; }

        public string ServiceProvidermileageId { get; set; }
        public string vehiclemileagegroupid { get; set; }
        public string Rangemileage { get; set; }


        public string ServiceProvidercost { get; set; }

        public string ServiceProviderparkingId { get; set; }

        public string ServiceProviderparkingprice { get; set; }
        public string parking { get; set; }
        public string parkingid { get; set; }


        public string ServiceProvidercoverId { get; set; }

        public string ServiceProvidercoverFactor { get; set; }
        public string vehiclecovertype { get; set; }
        public string vehiclecovertypeid { get; set; }

        public string ServiceProvidervtypeId { get; set; }

        public string ServiceProvidervtypeFactor { get; set; }
        public string vehicletype { get; set; }
        public string VehicleTypeId { get; set; }

        public string ServiceProviderlgaId { get; set; }

        public string lgaid { get; set; }
        public string lganame { get; set; }
        public string lgaprice { get; set; }

        public string ServiceProviderNoClaimId { get; set; }

        public string noclaimyrid { get; set; }
        public string noclaimyr { get; set; }
        public string noclaimfactor { get; set; }

        public string ServiceProviderdriverageId { get; set; }

        public string driverageid { get; set; }
        public string driveragerange { get; set; }
        public string driveragefactor { get; set; }
    }

    public class providermodellist
    {
        public List<ProviderModel> ServiceProvider { get; set; }
       // public List<providermodelmileage> ServiceProvidermileage { get; set; }


    }


    public class providermodelmileage
    {
        public string ServiceProvidermileageId { get; set; }
        public string vehiclemileagegroupid { get; set; }
        public string Rangemileage { get; set; }


        public string ServiceProvidercost { get; set; }
    }
}