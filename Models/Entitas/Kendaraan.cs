﻿namespace ShowRoomAPI.Models.Entitas
{
    public abstract class Kendaraan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
