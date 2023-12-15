using CalcHypto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Net.Http.Headers;
using System.Windows.Media.Animation;
using System.Security.Cryptography.X509Certificates;

namespace CalcHypto.DAL.Factories
{
    internal class SimulationFactory
    {
        private readonly string EMPLACEMENT_FICHIER = "Simulation.xml";

        public  List<Simulation> GetAllSimulation()
        {
            List<Simulation>? lesSimulations = new List<Simulation>();
            XmlSerializer ser = null;
            XmlReader reader = null;
            XmlWriter writer = null;
            System.Xml.Serialization.XmlSerializer mySerialiser = null;

            try
            {
                ser = new XmlSerializer(typeof(List<Simulation>));
                reader = XmlReader.Create(EMPLACEMENT_FICHIER);
                lesSimulations = (List<Simulation>)ser.Deserialize(reader);
            }
            catch (Exception e)
            {
               
                mySerialiser = new System.Xml.Serialization.XmlSerializer(new List<Simulation>().GetType());
                writer = XmlWriter.Create(EMPLACEMENT_FICHIER);
                mySerialiser.Serialize(writer, new List<Simulation>());
                throw new Exception("Creation d'un nouveau Fichier "+ e.Message );
            }
            finally
            {
                if (reader != null)
                reader.Close();
                if (writer != null)
                    writer.Close();
                
            }

            return lesSimulations;
        }

        public void WriteList(List<Simulation> lesSimulations)
        {
            XmlWriter writer = null;
            System.Xml.Serialization.XmlSerializer mySerialiser = null;
            try
            {
               mySerialiser  = new System.Xml.Serialization.XmlSerializer(lesSimulations.GetType());
                writer = XmlWriter.Create(EMPLACEMENT_FICHIER);
                mySerialiser.Serialize(writer, lesSimulations);
                
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                writer.Close();
            }

        }

        public Simulation SaveSimulation(Simulation simIN)
        {

            List<Simulation> lesSims = null;

            try
            {
                lesSims = GetAllSimulation();
            }
            catch (Exception e)
            {
                throw e;
            }
           

            foreach (Simulation simulation in lesSims)
            {
                if (simIN.Equal(simulation))
                {
                    lesSims.Remove(simulation);
                    lesSims.Add(simIN);
                    WriteList(lesSims);
                    return simIN;
                }
            }

            lesSims.Add(simIN);
            WriteList(lesSims);
            return simIN;
        }

        public void DeleteSimulation(Simulation simulationIn)
        {
            List<Simulation> lesSims = null;

            try
            {
                lesSims =  GetAllSimulation();
            }
            catch (Exception e)
            {
                throw e;
            }

            if (lesSims != null)
            {
                foreach (Simulation simulation in lesSims)
                {
                    if (simulationIn.Equal(simulation))
                    {
                        lesSims.Remove(simulation);
                        WriteList(lesSims);
                        return;
                    }
                }
            }

        }

    }
}
