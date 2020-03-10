using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace GettingStartedLib
{
    class ChequeReposytory : IChequeReposytory
    {
        string ConnectionString { get; }

        public ChequeReposytory()
        {
             ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            // ConnectionString = ConfigurationManager.ConnectionStrings["DupperDbConnection"].ConnectionString;
        }
        
        public List<Cheque> GetLastCheques(int lastChequeCount)
        {
            List<Cheque> chequesCollection = new List<Cheque>();
            
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Cheque cheque;
                var cheques1 = db.Query("dbo.get_cheques_pack", new { pack_size = lastChequeCount }, commandType: CommandType.StoredProcedure).ToList();
                foreach (var e in cheques1)
                {
                    var splitedArticles = e.articles.Split(';');

                    cheque = new Cheque() {      Id = e.cheque_id, 
                                             Number = e.cheque_number, 
                                               Summ = e.summ, 
                                           Discount = e.discount,
                                           Articles = splitedArticles };
                    
                    chequesCollection.Add(cheque);
                }
                
            }
           
            return chequesCollection;
        }

        public void SaveCheque(Cheque cheque)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Execute("dbo.save_cheques",new { cheque_id = cheque.Id, 
                                                cheque_number = cheque.Number, 
                                                         summ = cheque.Summ,
                                                     discount = cheque.Discount,
                                                     articles = string.Join(";", cheque.Articles)}, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
