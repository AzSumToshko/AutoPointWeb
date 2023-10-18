namespace AutoPoint.ViewModel.HomeVM
{
    public class EngineVM
    {
		public int id { get; set; }
		public int make_model_trim_id { get; set; }
        public string engine_type { get; set; }
        public string fuel_type { get; set; }
        public string cylinders { get; set; }
        public string size { get; set; }
        public int horsepower_hp { get; set; }
        public int horsepower_rpm { get; set; }
        public int torque_ft_lbs { get; set; }
        public int torque_rpm { get; set; }
        public int valves { get; set; }
        public string valve_timing { get; set; }
        public string cam_type { get; set; }
        public string drive_type { get; set; }
        public string transmission { get; set; }
    }
}
