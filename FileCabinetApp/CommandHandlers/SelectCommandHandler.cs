using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Selects records by input parameters.
    /// </summary>
    internal class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private string[] availableFieldsOfRecords = { "ID", "FIRSTNAME", "LASTNAME", "DATEOFBIRTH", "SSERIEOFPASSNUMBER", "PASSNUMBER", "BANKACCOUNT" };

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The instance of file cabinet service instance.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "SELECT")
            {
                this.Select(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        /// <summary>
        /// Identificates of valid field of record.
        /// </summary>
        /// <param name="fieldsOfRecordForSearch">The list of input paremeters.</param>
        /// <returns>The list of valid fields name of record and its value.</returns>
        private static List<Tuple<string, string>> IdentificateOfRecordFields(string[] fieldsOfRecordForSearch)
        {
            List<Tuple<string, string>> result = new ();

            foreach (string i in fieldsOfRecordForSearch)
            {
                string[] currentFields = i.Split(new char[] { ' ', '=', '\"' }, StringSplitOptions.RemoveEmptyEntries);
                currentFields[1] = currentFields[1].Trim('\'', '\"');
                if (currentFields.Length != 2)
                {
                    continue;
                }

                switch (currentFields[0].ToUpperInvariant())
                {
                    case "ID":
                        result.Add(new Tuple<string, string>("Id", currentFields[1]));

                        break;
                    case "FIRSTNAME":
                        result.Add(new Tuple<string, string>("FirstName", currentFields[1]));

                        break;
                    case "LASTNAME":
                        result.Add(new Tuple<string, string>("LastName", currentFields[1]));

                        break;
                    case "DATEOFBIRTH":
                        result.Add(new Tuple<string, string>("DateOfBirth", currentFields[1]));

                        break;
                    case "SERIEOFPASSNUMBER":
                        result.Add(new Tuple<string, string>("SerieOfPassNumber", currentFields[1]));

                        break;
                    case "PASSNUMBER":
                        result.Add(new Tuple<string, string>("PassNumber", currentFields[1]));

                        break;
                    case "BANKACCOUNT":
                        result.Add(new Tuple<string, string>("BankAccount", currentFields[1]));

                        break;
                    default:
                        continue;
                }
            }

            return result;
        }

        private void Select(string parameters)
        {
            string[] commands = parameters.Split("where", 2, StringSplitOptions.TrimEntries);

            if (commands[0].Length == 0)
            {
                Console.WriteLine("Enter parameters for display");
                return;
            }

            string[] fieldsOfRecordForDisplay = commands[0].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] validfieldsOfRecordForDisplay = fieldsOfRecordForDisplay.Where(field => this.availableFieldsOfRecords.Contains(field.ToUpperInvariant())).ToArray<string>();

            if (commands.Length < 2)
            {
                this.fileCabinetService.SelectRecords(new List<Tuple<string, string>>(), validfieldsOfRecordForDisplay, string.Empty);
                return;
            }

            string orderOfFindRecords = string.Empty;
            if (commands[1].Contains("or", StringComparison.InvariantCulture) || commands[1].Contains("and", StringComparison.InvariantCulture))
            {
                orderOfFindRecords = commands[1].Contains("or", StringComparison.InvariantCulture) ? "or" : "and";
            }

            string[] parametersForFindRecords = commands[1].Split(new string[] { ",", orderOfFindRecords }, StringSplitOptions.TrimEntries);
            List<Tuple<string, string>> fieldsForSort = IdentificateOfRecordFields(parametersForFindRecords);

            this.fileCabinetService.SelectRecords(fieldsForSort, validfieldsOfRecordForDisplay, orderOfFindRecords);
        }
    }
}
