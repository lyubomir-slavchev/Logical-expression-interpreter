using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace KursovaRabotaV2
{

    class Program
    {
        static FixedSizeGenericHashTable<string, Node> funcTable = new FixedSizeGenericHashTable<string, Node>(100);
        static FixedSizeGenericHashTable<string, string[]> funcTableParams = new FixedSizeGenericHashTable<string, string[]>(100);
        static FixedSizeGenericHashTable<Node, FixedSizeGenericHashTable<string, int>> nodeResults = new FixedSizeGenericHashTable<Node, FixedSizeGenericHashTable<string, int>>(100);

        static void Main(string[] args)
        {
            
            Stack<Node> stack = new Stack<Node>();
            Console.WriteLine("Enter command:");
            while (true) { 
            string userInput = Console.ReadLine();
            HandleInput(userInput);
                }

        }

        static void HandleInput(string input)
        {
            string command = ReadUntilWhitespace(input);
            command = command.ToUpper();
            
            switch (command)
            {
                case "DEFINE":
                    string funcName = ReadUntilOpeningBracket(input,command);
                    string[] parameters = ReadFunctionParameters(input);
                    string expression = ReadExpression(input);
                    string[] body = ReadFunctionBody(input);
                    DefineFunction(funcName, parameters, expression,body);
                    break;
                case "SOLVE":
                    string funcNameSolve = ReadUntilOpeningBracket(input,command);
                    string[] parametersSolve = ReadFunctionParameters(input);
                    string expressionSolve = ReadExpression(input);
                    SolveFunction(funcNameSolve, parametersSolve, expressionSolve);
                    break;
                case "ALL":
                    string funcNameAll = "";
                    for (int i = 4; i < input.Length;i++) { 
                        funcNameAll += input[i];
                    }
                    AllFunction(funcNameAll);
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }

        static string ReadUntilWhitespace(string input)
        {
            string result = "";
            foreach (char c in input)
            {
                if (c == ' ')
                    break;
                result += c;
            }
            return result;
        }

        static string ReadUntilOpeningBracket(string input, string command)
        {
            string result = "";
            int startIndex = 0;
            for (int j = 0; j < input.Length; j++)
            {
                if (input[j] == '(')
                {
                    startIndex = j;
                    break;
                }
            }
            
            for (int i = command.Length+1; i < startIndex; i++)
            {
                result += input[i];
            }
            return result;
        }

        static string ReadUntilOpeningBracketBody(string input, int startIndex)
        {
            string result = "";
            for (int i = startIndex; i < input.Length; i++)
            {
                if (input[i] == '(') {
                    break;
                }
                result += input[i];
            }
            return result;
        }

        static string[] ReadFunctionParameters(string input)
        {
            List<string> parameters = new List<string>();
            string currentParam = "";
            bool insideParameter = false;
            int bracketCount = 0;

            foreach (char c in input)
            {
                if (c == '(')
                {
                    insideParameter = true;
                    bracketCount++;
                }
                else if (c == ')')
                {
                    bracketCount--;
                    if (bracketCount == 0) 
                    {
                        parameters.Add(currentParam.Trim()); 
                        break; 
                    }
                }
                else if (c == ',' && bracketCount == 1) 
                {
                    parameters.Add(currentParam.Trim()); 
                    currentParam = ""; 
                    continue;
                }
                else if (insideParameter) 
                {
                    currentParam += c; 
                }
            }

            return parameters.ToArray();
        }

        static string[] ReadFunctionBody(string input)
        {
            List<string> body = new List<string>();
            string currentBodyMember = "";
            bool insideBody = false;
            int qMarkCount = 0;

            foreach (char c in input)
            {
                if (c == '"' && qMarkCount == 0)
                {
                    insideBody = true;
                    qMarkCount++;
                }
                else if (c == '"' && qMarkCount == 1)
                {
                    body.Add(currentBodyMember.Trim());
                    break;

                }
                else if (c == ' ') {
                    continue;
                }
                else if (insideBody)
                {
                    currentBodyMember += c;
                    body.Add(currentBodyMember.Trim());
                    currentBodyMember = "";
                }
              
            }

            return body.ToArray();
        }

        static string ReadExpression(string input)
        {
            StringBuilder expression = new StringBuilder();
            int openParenthesisCount = 0; 
            int closeParenthesisCount = 0; 

            bool insideExpression = false;

            foreach (char c in input)
            {
                if (c == '(')
                {
                    openParenthesisCount++;
                    insideExpression = true;
                }
                else if (c == ')')
                {
                    closeParenthesisCount++;
                }

                if (insideExpression)
                {
                    expression.Append(c); 
                }
               
                if (openParenthesisCount > 0 && openParenthesisCount == closeParenthesisCount)
                {
                    break; 
                }
            }

            return expression.ToString().Trim();
        }

        static void DefineFunction(string funcName, string[] parameters, string expression, string[] body)
        {
            string strBody = ""; 
            foreach (string c in body) {
            strBody += c;
            }
            Node currentNode;
            Stack<Node> stack = new Stack<Node>();
            Node operand1;
            Node operand2;
            BinaryTree tree = new BinaryTree();

            string innerFuncName = "";
            string[] innerParameters = new string[] { };


            for (int i = 0; i < body.Length; i++)
            {
                if (body[i] == "f" && body[i + 1] == "u")
                {
                    innerFuncName = ReadUntilOpeningBracketBody(strBody, i);
                    innerParameters = ReadFunctionParameters(strBody);
                    string[] previousFuncParams = funcTableParams.Find(innerFuncName);
                    if (funcTable.ContainsKey(innerFuncName) && previousFuncParams.Length == innerParameters.Length)
                    {
                        foreach (string p in innerParameters)
                        {
                            if (!parameters.Contains(p))
                            {
                                throw new Exception("Invalid arguments");
                            }
                        }

                        stack.Push(funcTable.Find(innerFuncName));

                    }

                    for (int k = i; k < body.Length; k++)
                    {
                        if (body[k] == ")")
                        {
                            i = k ;
                            break;
                        }
                    }
                }
                else if (body[i] == " ")
                {
                    continue;
                }
               else if (body[i] == "&" || body[i] == "|")
                {
                    operand1 = stack.Pop();
                    operand2 = stack.Pop();
                    currentNode = new Node(body[i], operand2, operand1);
                    stack.Push(currentNode);
                }
                else if (body[i] == "!")
                {
                    operand1 = stack.Pop();
                    currentNode = new Node(body[i], operand1, null);
                    stack.Push(currentNode);
                }
                else if (body[i] == "(" || body[i] == ")" || body[i] == " " || body[i] == "")
                {
                    continue;
                }
                else if (parameters.Contains(body[i]))
                {
                    currentNode = new Node(body[i], null, null);
                    stack.Push(currentNode);
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            
            funcTableParams.Add(funcName,parameters);
            funcTable.Add(funcName, stack.Pop());

            
        }

        static int SolveNode(Node currentNode, string[] parameters) {
            string parametersString = ""; 
            for (int i = 0; i < parameters.Length; i++) {
                   parametersString += parameters[i];
            }
            if (nodeResults.ContainsKey(currentNode)) {
             FixedSizeGenericHashTable<string, int> parametersTable = nodeResults.Find(currentNode);
                if (parametersTable.ContainsKey(parametersString)) {
                    return parametersTable.Find(parametersString);
                }    
            }
            if (currentNode.element == "&") {
                int result =  SolveNode(currentNode.left, parameters) & SolveNode(currentNode.right, parameters);
                if (!nodeResults.ContainsKey(currentNode))
                {
                    FixedSizeGenericHashTable<string, int> newTable = new FixedSizeGenericHashTable<string, int>(50);
                    nodeResults.Add(currentNode, newTable);
                }
                FixedSizeGenericHashTable<string, int> currentTable = nodeResults.Find(currentNode);
                currentTable.Add(parametersString, result);
                return result;
            }
            if (currentNode.element == "|")
            {
                int result =  SolveNode(currentNode.left, parameters) | SolveNode(currentNode.right, parameters);
                if (!nodeResults.ContainsKey(currentNode))
                {
                    FixedSizeGenericHashTable<string, int> newTable = new FixedSizeGenericHashTable<string, int>(50);
                    nodeResults.Add(currentNode, newTable);
                }
                FixedSizeGenericHashTable<string, int> currentTable = nodeResults.Find(currentNode);
                currentTable.Add(parametersString, result);
                return result;
            }
            if (currentNode.element == "!")
            {
                int result =SolveNode(currentNode.left, parameters) ^ 1;
                if (!nodeResults.ContainsKey(currentNode))
                {
                    FixedSizeGenericHashTable<string, int> newTable = new FixedSizeGenericHashTable<string, int>(50);
                    nodeResults.Add(currentNode, newTable);
                }
                FixedSizeGenericHashTable<string, int> currentTable = nodeResults.Find(currentNode);
                currentTable.Add(parametersString, result);
                return result;
            }
            int pos = (int)currentNode.element[0] - 97;
            return int.Parse(parameters[pos]);
        }

        static void SolveFunction(string funcNameSolve, string[] parametersSolve, string expressionSolve) {
            if (funcTable.Find(funcNameSolve) == null)
            {
                Console.WriteLine("ERROR FUNCTION NAME NOT DEFINED");
            }
            else {
                if (funcTableParams.Find(funcNameSolve).Length != parametersSolve.Length) {
                    Console.WriteLine("ERROR IN PARAMETERS");
                }
            }

            int result =  SolveNode(funcTable.Find(funcNameSolve), parametersSolve);
            Console.WriteLine(result);
        }


        static void AllFunction(string funcNameAll) 
         {
            if (funcTable.Find(funcNameAll) == null)
            {
                Console.WriteLine("ERROR FUNCTION NAME NOT DEFINED");
            }

            int parametersLength = funcTableParams.Find(funcNameAll).Length;
            string[][] combinations = GenerateCombinations(parametersLength);
            for (int i = 0; i < combinations.Length; i++) {
                foreach (string j in combinations[i])
                {
                    Console.Write(j + " ");
                }
                Console.Write(": " + SolveNode(funcTable.Find(funcNameAll), combinations[i]));
                Console.WriteLine();
            }

            
        }

        static string[][] GenerateCombinations(int n)
        {
            int totalCombinations = (int)Math.Pow(2, n);
            string[][] result = new string[totalCombinations][];

            for (int i = 0; i < totalCombinations; i++)
            {
                result[i] = new string[n];
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = ((i >> (n - j - 1)) & 1).ToString();
                }
            }

            return result;
        }
    }
}
