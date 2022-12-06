namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        private Dictionary<string, Doctor> doctors = new Dictionary<string, Doctor>();
        private Dictionary<string, Patient> patients = new Dictionary<string, Patient>();
        private Dictionary<string, List<Patient>> dMap = new Dictionary<string, List<Patient>>();
        private Dictionary<string, Doctor> pMap = new Dictionary<string, Doctor>(); 


        public void AddDoctor(Doctor d)
        {
            if (this.doctors.ContainsKey(d.Name))
            {
                throw new ArgumentException();
            }

            this.doctors.Add(d.Name, d);
            this.dMap.Add(d.Name, new List<Patient>());
        }

        public void AddPatient(Doctor d, Patient p)
        {
            if (!this.doctors.ContainsKey(d.Name))
            {
                throw new ArgumentException();
            }

            patients.Add(p.Name, p);
            dMap[d.Name].Add(p);
            pMap.Add(p.Name, d);
        }

        public IEnumerable<Doctor> GetDoctors() => doctors.Values;

        public IEnumerable<Patient> GetPatients() => this.patients.Values;

        public bool Exist(Doctor d) => this.doctors.ContainsKey(d.Name);

        public bool Exist(Patient p) => this.patients.ContainsKey(p.Name);

        public Doctor RemoveDoctor(string name)
        {
            if (this._doctors.ContainsKey(name) == false) throw new ArgumentException();

            var removedDoctor = this._doctors[name];
            var patientsToRemove = this._dMap[name];

            this._doctors.Remove(name);
            this._dMap.Remove(name);
            foreach (var p in patientsToRemove)
            {
                this._patients.Remove(p);
                this._pMap.Remove(p);
            }

            return removedDoctor;
        }

        public void ChangeDoctor(Doctor from, Doctor to, Patient p)
        {
            if (this.Exist(from) == false || this.Exist(to) == false || this.Exist(p) == false) throw new ArgumentException();
            this._dMap[from.Name].Remove(p.Name);
            this._dMap[to.Name].Add(p.Name);
            this._pMap[p.Name] = to.Name;
        }

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry) => this.GetDoctors().Where(d => d.Popularity == populariry);

        public IEnumerable<Patient> GetPatientsByTown(string town) => this.GetPatients().Where(p => p.Town == town);

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi) => this.GetPatients().Where(p => lo <= p.Age && p.Age <= hi);

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc() => this.GetDoctors().OrderByDescending(d => this._dMap[d.Name].Count).ThenBy(d => d.Name);

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge() => this.GetPatients().OrderBy(p => this._doctors[this._pMap[p.Name]].Popularity).ThenByDescending(p => p.Height).ThenBy(p => p.Age);
    }
}
