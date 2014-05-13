using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.IManagers
{
    public interface IRevisionManager
    {
        Revision getRevisionById(int revisionID);
        List<Revision> getRevisionsByMarque(int marqueID);
        List<Revision> getRevisionsByModele(int modeleID);
        List<Revision> getAllRevisions();
        bool updateRevision(Revision revision, int modeleID, int marqueID);
        bool deleteRevision(int revisionID);
        int createRevision(Revision revision, int modeleID, int marqueID);
    }
}
