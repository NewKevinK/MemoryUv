namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserGame")]
    public partial class UserGame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserGame()
        {
            Friend = new HashSet<Friend>();
            FriendRequest = new HashSet<FriendRequest>();
            StatisticUser = new HashSet<StatisticUser>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(30)]
        public string email { get; set; }

        [Required]
        [StringLength(255)]
        public string password { get; set; }

        [Required]
        [StringLength(20)]
        public string nametag { get; set; }

        [StringLength(20)]
        public string status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friend> Friend { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendRequest> FriendRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StatisticUser> StatisticUser { get; set; }
    }
}
