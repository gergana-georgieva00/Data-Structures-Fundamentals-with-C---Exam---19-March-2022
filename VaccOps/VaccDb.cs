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


        public void AddDoctor(Doctor d)
        {
            if (this.doctors.ContainsKey(d.Name))
            {
                throw new ArgumentException();
            }

            this.doctors.Add(d.Name, d);
        }

        public void AddPatient(Doctor d, Patient p)
        {
            if (!this.doctors.ContainsKey(d.Name))
            {
                throw new ArgumentException();
            }

            patients.Add(p.Name, p);
            d.Patients.Add(p);
            p.Doctor = d;
        }

        public IEnumerable<Doctor> GetDoctors() => doctors.Values;

        public IEnumerable<Patient> GetPatients() => patients.Values;

        public bool Exist(Doctor d) => this.doctors.ContainsKey(d.Name);

        public bool Exist(Patient p) => this.patients.ContainsKey(p.Name);

        public Doctor RemoveDoctor(string name)
        {
            if (!this.doctors.ContainsKey(name))
            {
                throw new ArgumentException();
            }

            var removedDoc = doctors[name];
            doctors.Remove(name);
            foreach (var patient in removedDoc.Patients)
            {
                patients.Remove(patient.Name);
            }

            return removedDoc;
        }

        public void ChangeDoctor(Doctor from, Doctor to, Patient p)
        {
            if (!this.doctors.ContainsKey(from.Name))
            {
                throw new ArgumentException();
            }
            if (!this.doctors.ContainsKey(to.Name))
            {
                throw new ArgumentException();
            }
            if (!this.patients.ContainsKey(p.Name))
            {
                throw new ArgumentException();
            }

            from.Patients.Remove(p);
            to.Patients.Add(p);
            p.Doctor = to;
        }

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
            => this.doctors.Values.Where(d => d.Popularity == populariry);

        public IEnumerable<Patient> GetPatientsByTown(string town)
            => this.patients.Values.Where(p => p.Town == town);

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
            => this.patients.Values.Where(p => p.Age >= lo && p.Age <= hi);

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
            => this.doctors.Values.OrderByDescending(d => d.Patients.Count).ThenBy(d => d.Name);

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
            => this.patients.Values.OrderBy(p => p.Doctor.Popularity).ThenByDescending(p => p.Height).ThenBy(p => p.Age);
    }
}
