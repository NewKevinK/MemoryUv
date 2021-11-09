namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatisticUser")]
    public partial class StatisticUser
    {
        public int idUser { get; set; }

        public int? totalGames { get; set; }

        public int? totalWins { get; set; }

        public int? totalDefeat { get; set; }

        public int? totalScore { get; set; }

        public int id { get; set; }

        [StringLength(10)]
        public string nameTag { get; set; }

        public virtual UserGame UserGame { get; set; }
    }
}
