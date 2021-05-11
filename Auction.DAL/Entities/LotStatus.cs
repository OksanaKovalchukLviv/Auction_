namespace Auction.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LotStatus
    {
        public int LotStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string LotStatusName { get; set; }
    }
}
