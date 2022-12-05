namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        private List<Doctor> doctors;
        private List<Patient> patients;

        public VaccDb()
        {
            doctors = new List<Doctor>();
            patients = new List<Patient>();
        }

        public void AddDoctor(Doctor doctor)
        {
            if (this.doctors.Contains(doctor))
            {
                throw new ArgumentException();
            }

            this.doctors.Add(doctor);
        }

        public void AddPatient(Doctor doctor, Patient patient)
        {
            if (!this.doctors.Contains(doctor) || this.patients.Contains(patient))
            {
                throw new ArgumentException();
            }

            doctor.Patients.Add(patient);
            this.patients.Add(patient);
            patient.Doctor = doctor;
        }

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if (!this.doctors.Contains(oldDoctor))
            {
                throw new ArgumentException();
            }
            if (!this.doctors.Contains(newDoctor))
            {
                throw new ArgumentException();
            }
            if (!this.patients.Contains(patient))
            {
                throw new ArgumentException();
            }

            //var patientsToMove = oldDoctor.Patients;
            oldDoctor.Patients.Remove(patient);
            newDoctor.Patients.Add(patient);
            patient.Doctor = newDoctor;
        }

        public bool Exist(Doctor doctor)
            => this.doctors.Contains(doctor);

        public bool Exist(Patient patient)
            => this.patients.Contains(patient);

        public IEnumerable<Doctor> GetDoctors()
            => this.doctors;

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
            => doctors.Where(d => d.Popularity == populariry);

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
            => this.doctors.OrderByDescending(d => d.Patients.Count).ThenBy(d => d.Name);

        public IEnumerable<Patient> GetPatients()
            => this.patients;

        public IEnumerable<Patient> GetPatientsByTown(string town)
            => this.patients.Where(p => p.Town == town);

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
            => this.patients.Where(p => p.Age >= lo && p.Age <= hi);

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
            => this.patients.OrderBy(p => p.Doctor.Popularity).ThenByDescending(p => p.Height).ThenBy(p => p.Age);

        public Doctor RemoveDoctor(string name)
        {
            if (!this.doctors.Any(d => d.Name == name))
            {
                throw new ArgumentException();
            }

            var doctor = this.doctors.Find(d => d.Name == name);
            var patientsToRemove = doctor.Patients;
            this.doctors.Remove(doctor);
            foreach (var patient in patientsToRemove)
            {
                patients.Remove(patient);
            }

            return doctor;
        }
    }
}
