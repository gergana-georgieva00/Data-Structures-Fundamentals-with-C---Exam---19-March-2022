namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        Dictionary<string, Doctor> patientsByName;
        Dictionary<string, List<Patient>> doctorsByName;
        HashSet<Doctor> doctors;

        public VaccDb()
        {
            patientsByName = new Dictionary<string, Doctor>();
            doctorsByName = new Dictionary<string, List<Patient>>();
            doctors = new HashSet<Doctor>();
        }

        public void AddDoctor(Doctor doctor)
        {
            if (this.doctorsByName.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            this.doctorsByName.Add(doctor.Name, new List<Patient>());
            doctors.Add(doctor);
        }

        public void AddPatient(Doctor doctor, Patient patient)
        {
            if (!this.doctorsByName.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            patientsByName.Add(patient.Name, doctor);
            doctorsByName[doctor.Name].Add(patient);
            doctor.Patients.Add(patient);
        }

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if (!this.doctorsByName.ContainsKey(oldDoctor.Name))
            {
                throw new ArgumentException();
            }
            if (!this.doctorsByName.ContainsKey(newDoctor.Name))
            {
                throw new ArgumentException();
            }
            if (!patientsByName.ContainsKey(patient.Name))
            {
                throw new ArgumentException();
            }

            doctorsByName[oldDoctor.Name].Remove(patient);
            doctorsByName[newDoctor.Name].Add(patient);
            patientsByName[patient.Name] = newDoctor;
            oldDoctor.Patients.Remove(patient);
            newDoctor.Patients.Add(patient);
        }

        public bool Exist(Doctor doctor)
            => this.doctorsByName.ContainsKey(doctor.Name);

        public bool Exist(Patient patient)
            => this.patientsByName.ContainsKey(patient.Name);

        public IEnumerable<Doctor> GetDoctors()
            => this.doctorsByName.Keys;

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

            doctorPatients.Remove(doctor);
            foreach (var patient in patientsToRemove)
            {
                patientDoctor.Remove(patient);
            }

            return doctor;
        }
    }
}
