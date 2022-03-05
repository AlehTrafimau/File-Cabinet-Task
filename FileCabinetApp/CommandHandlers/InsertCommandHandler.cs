namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// dfdf.
    /// </summary>
    internal class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The instance of file cabinet service instance.</param>
        /// <param name="recordValidator">Sets rules of validation.</param>
        public InsertCommandHandler(IFileCabinetService fileCabinetService, IRecordValidator recordValidator)
            : base(fileCabinetService)
        {
            this.recordValidator = recordValidator;
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "INSERT")
            {
                this.Insert(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private void Insert(string parameters)
        {
            string[] fieldsValues = parameters.Split("values");
            string[] fields = fieldsValues[0].Split(new char[] { '\\', ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            string[] values = fieldsValues[1].Split(new char[] { '\\', ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            if (fieldsValues.Length != 2 || fields.Length != values.Length)
            {
                Console.WriteLine("Invalid commamd. Enter \"values\" command");
                return;
            }

            Dictionary<string, string> recordsFields = new ();
            for (int i = 0; i < fields.Length; i++)
            {
                recordsFields.Add(fields[i].ToUpperInvariant(), values[i]);
            }

            if (recordsFields.ContainsKey("ID") && recordsFields.ContainsKey("DATEOFBIRTH") && recordsFields.ContainsKey("SERIEOFPASSNUMBER") && recordsFields.ContainsKey("PASSNUMBER") && recordsFields.ContainsKey("BANKACCOUNT") && recordsFields.ContainsKey("LASTNAME") && recordsFields.ContainsKey("FIRSTNAME"))
            {
                Tuple<bool, string, int> idconverter = StringConverter.IntegerConvert(recordsFields["ID"]);
                Tuple<bool, string, DateTime> dateOfBirthConverter = StringConverter.DateTimeConvert(recordsFields["DATEOFBIRTH"]);
                Tuple<bool, string, char> serieOfPassNumberConverter = StringConverter.CharConvert(recordsFields["SERIEOFPASSNUMBER"]);
                Tuple<bool, string, short> passNumberConverter = StringConverter.ShortConvert(recordsFields["PASSNUMBER"]);
                Tuple<bool, string, decimal> bankAccountConverter = StringConverter.DecimalConvert(recordsFields["BANKACCOUNT"]);
                string firstName = recordsFields["FIRSTNAME"];
                string lastName = recordsFields["LASTNAME"];
                int id = default;
                DateTime dateOfBirth = default;
                char serieOfPassNumber = default;
                short passNumber = default;
                decimal bankAccount = default;

                if (idconverter.Item1)
                {
                    id = idconverter.Item3;
                }
                else
                {
                    Console.WriteLine(idconverter.Item2);
                }

                if (dateOfBirthConverter.Item1)
                {
                    dateOfBirth = dateOfBirthConverter.Item3;
                }
                else
                {
                    Console.WriteLine(dateOfBirthConverter.Item2);
                }

                if (dateOfBirthConverter.Item1)
                {
                    dateOfBirth = dateOfBirthConverter.Item3;
                }
                else
                {
                    Console.WriteLine(dateOfBirthConverter.Item2);
                }

                if (serieOfPassNumberConverter.Item1)
                {
                    serieOfPassNumber = serieOfPassNumberConverter.Item3;
                }
                else
                {
                    Console.WriteLine(serieOfPassNumberConverter.Item2);
                }

                if (passNumberConverter.Item1)
                {
                    passNumber = passNumberConverter.Item3;
                }
                else
                {
                    Console.WriteLine(passNumberConverter.Item2);
                }

                if (bankAccountConverter.Item1)
                {
                    bankAccount = bankAccountConverter.Item3;
                }
                else
                {
                    Console.WriteLine(bankAccountConverter.Item2);
                }

                FileCabinetRecord recordForValidate = new (id, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount);
                Tuple<bool, string> validator = this.recordValidator.ValidateParameters(recordForValidate);

                if (validator.Item1 == false)
                {
                    Console.WriteLine($"\tValidation failed:");
                    Console.Write(validator.Item2);
                    Console.WriteLine($"\tPlease, correct your input.");
                }

                this.fileCabinetService.InsertRecord(recordForValidate);
                Console.WriteLine($"Record #{id} is inserted.");
            }
        }
    }
}