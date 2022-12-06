namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        Dictionary<Patient, Doctor> patientDoctor;
        Dictionary<Doctor, List<Patient>> doctorPatients;

        public VaccDb()
        {
            //doctors = new HashSet<Doctor>();
            //patients = new HashSet<Patient>();
            patientDoctor = new Dictionary<Patient, Doctor>();
            doctorPatients = new Dictionary<Doctor, List<Patient>>();
        }

        public void AddDoctor(Doctor doctor)
        {
            if (this.doctorPatients.ContainsKey(doctor))
            {
                throw new ArgumentException();
            }

            //this.doctors.Add(doctor);
            this.doctorPatients.Add(doctor, new List<Patient>());
        }

        public void AddPatient(Doctor doctor, Patient patient)
        {
            if (!this.doctorPatients.ContainsKey(doctor))
            {
                throw new ArgumentException();
            }

            patientDoctor.Add(patient, doctor);
            doctorPatients[doctor].Add(patient);
        }

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if (!this.doctorPatients.ContainsKey(oldDoctor))
            {
                throw new ArgumentException();
            }
            if (!this.doctorPatients.ContainsKey(newDoctor))
            {
                throw new ArgumentException();
            }
            //if (!this.patients.Contains(patient))
            //{
            //    throw new ArgumentException();
            //}
            if (!patientDoctor.ContainsKey(patient))
            {
                throw new ArgumentException();
            }

            //var patientsToMove = oldDoctor.Patients;
            doctorPatients[oldDoctor].Remove(patient);
            doctorPatients[newDoctor].Add(patient);
            patientDoctor[patient] = newDoctor;
        }

        public bool Exist(Doctor doctor)
            => this.doctorPatients.ContainsKey(doctor);

        public bool Exist(Patient patient)
            => this.patientDoctor.ContainsKey(patient);

        public IEnumerable<Doctor> GetDoctors()
            => this.doctorPatients.Keys;

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
            => doctorPatients.Keys.Where(d => d.Popularity == populariry);

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
            => this.doctorPatients.OrderByDescending(kvp => kvp.Value.Count).ThenBy(kvp => kvp.Key.Name).ToDictionary(x => x.Key, x => x.Value).Keys;

        public IEnumerable<Patient> GetPatients()
            => this.patientDoctor.Keys;

        public IEnumerable<Patient> GetPatientsByTown(string town)
            => this.patientDoctor.Keys.Where(p => p.Town == town);

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
            => this.patientDoctor.Keys.Where(p => p.Age >= lo && p.Age <= hi);

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
            => this.patientDoctor.OrderBy(p => p.Value.Popularity).ThenByDescending(p => p.Key.Height).ThenBy(p => p.Key.Age).ToDictionary(x => x.Key, x => x.Value).Keys;

        public Doctor RemoveDoctor(string name)
        {
            if (!this.doctorPatients.Keys.Any(d => d.Name == name))
            {
                throw new ArgumentException();
            }

            var doctor = this.doctorPatients.Keys.First(d => d.Name == name);
            var patientsToRemove = doctorPatients[doctor];
            //this.doctors.Remove(doctor);
            //foreach (var patient in patientsToRemove)
            //{
            //    patients.Remove(patient);
            //}

            doctorPatients.Remove(doctor);
            foreach (var patient in patientsToRemove)
            {
                patientDoctor.Remove(patient);
            }

            return doctor;
        }
    }
}
