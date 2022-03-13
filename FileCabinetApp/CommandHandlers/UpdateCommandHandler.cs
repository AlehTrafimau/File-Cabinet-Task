using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Updates the record for input data.
    /// </summary>
    internal class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The instance of file cabinet service instance.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "UPDATE")
            {
                this.Update(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private static FileCabinetRecord IndificateOfFields(string parameters)
        {
            FileCabinetRecord result = new ();
            string[] ipdateFields = parameters.Split(new string[] { ",", "and" }, StringSplitOptions.TrimEntries);
            foreach (string i in ipdateFields)
            {
                string[] field = i.Split(new char[] { ' ', '=', '\"' }, StringSplitOptions.RemoveEmptyEntries);
                field[1] = field[1].Trim('\'', '\"');
                if (field.Length != 2)
                {
                    continue;
                }

                switch (field[0].ToUpperInvariant())
                {
                    case "ID":
                        bool isDigit = int.TryParse(field[1], out int valueOfId);
                        if (isDigit)
                        {
                            result.Id = valueOfId;
                        }
                        else
                        {
                            Console.WriteLine("Id value must be a digit");
                        }

                        break;
                    case "FIRSTNAME":
                        result.FirstName = field[1];

                        break;
                    case "LASTNAME":
                        result.LastName = field[1];

                        break;
                    case "DATEOFBIRTH":
                        Tuple<bool, string, DateTime> date = StringConverter.DateTimeConvert(field[1]);
                        if (date.Item1)
                        {
                            result.DateOfBirth = date.Item3;
                        }
                        else
                        {
                            Console.WriteLine("Records with this field are not found");
                        }

                        break;
                    case "SERIEOFPASSNUMBER":
                        Tuple<bool, string, char> serieOfPassnumber = StringConverter.CharConvert(field[1]);
                        if (serieOfPassnumber.Item1)
                        {
                            result.SerieOfPassNumber = serieOfPassnumber.Item3;
                        }
                        else
                        {
                            Console.WriteLine(serieOfPassnumber.Item2);
                        }

                        break;
                    case "PASSNUMBER":
                        Tuple<bool, string, short> passNumber = StringConverter.ShortConvert(field[1]);

                        if (passNumber.Item1)
                        {
                            result.PassNumber = passNumber.Item3;
                        }
                        else
                        {
                            Console.WriteLine(passNumber.Item2);
                        }

                        break;
                    case "BANKACCOUNT":
                        Tuple<bool, string, decimal> bankAccount = StringConverter.DecimalConvert(field[1]);
                        if (bankAccount.Item1)
                        {
                            result.BankAccount = bankAccount.Item3;
                        }
                        else
                        {
                            Console.WriteLine(bankAccount.Item2);
                        }

                        break;
                    default:
                        Console.WriteLine($"The record haven't got this field {field[0]}");
                        break;
                }
            }

            return result;
        }

        private void Update(string parameters)
        {
            string[] commands = parameters.Split("set");
            if (commands.Length < 2 && commands[0].ToUpperInvariant() != "SET")
            {
                Console.WriteLine($"Invalid command. Correct format: \"update set \\parameters\\ where \\parameters\\ and \\parameters\\\"");
                return;
            }

            string[] vs = commands[1].Split("where");
            if (vs.Length != 2)
            {
                Console.WriteLine($"Invalid parameters {parameters}");
                return;
            }

            string updateFields = vs[0];
            string valueOfField = vs[1];
            FileCabinetRecord update = IndificateOfFields(updateFields);
            FileCabinetRecord findConditions = IndificateOfFields(valueOfField);

            this.fileCabinetService.Update(update, findConditions);
        }
    }
}
