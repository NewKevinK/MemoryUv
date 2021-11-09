namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Friend")]
    public partial class Friend
    {
        public int idUser { get; set; }

        public int idFriend { get; set; }

        public int id { get; set; }

        public virtual UserGame UserGame { get; set; }
    }
}
