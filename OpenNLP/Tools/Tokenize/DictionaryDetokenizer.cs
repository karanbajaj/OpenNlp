﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenNLP.Tools.Tokenize
{
    public class DictionaryDetokenizer : IDetokenizer
    {
        private readonly Dictionary<string, DetokenizationOperation> _tokenToDetokenizationOperation;

        
        // Constructors -----------------

        public DictionaryDetokenizer()
        {
            _tokenToDetokenizationOperation = new Dictionary<string, DetokenizationOperation>()
            {
                // Punctuation
                {".", DetokenizationOperation.MERGE_TO_LEFT},
                {"...", DetokenizationOperation.MERGE_TO_LEFT},
                {",", DetokenizationOperation.MERGE_TO_LEFT},
                {"!", DetokenizationOperation.MERGE_TO_LEFT},
                {"?", DetokenizationOperation.MERGE_TO_LEFT},
                {";", DetokenizationOperation.MERGE_TO_LEFT},
                {"(", DetokenizationOperation.MERGE_TO_RIGHT},
                {")", DetokenizationOperation.MERGE_TO_LEFT},
                {"[", DetokenizationOperation.MERGE_TO_RIGHT},
                {"]", DetokenizationOperation.MERGE_TO_LEFT},
                {"\"", DetokenizationOperation.RIGHT_LEFT_MATCHING},
                {"-", DetokenizationOperation.MERGE_BOTH_IF_SURROUNDED_BY_WORDS},
                // Contractions
                {"'t", DetokenizationOperation.MERGE_TO_LEFT},
                {"'m", DetokenizationOperation.MERGE_TO_LEFT},
                {"'s", DetokenizationOperation.MERGE_TO_LEFT},
                {"'re", DetokenizationOperation.MERGE_TO_LEFT},
                {"'ve", DetokenizationOperation.MERGE_TO_LEFT},
                {"'d", DetokenizationOperation.MERGE_TO_LEFT},
                {"'ll", DetokenizationOperation.MERGE_TO_LEFT},
                // Currencies
                {"$", DetokenizationOperation.MERGE_TO_RIGHT},
                {"€", DetokenizationOperation.MERGE_TO_LEFT},
            };
        }

        public DictionaryDetokenizer(Dictionary<string, DetokenizationOperation> dict)
        {
            this._tokenToDetokenizationOperation = dict;
        }

        
        // Methods ---------------------

        private readonly static Regex WordRegex = new Regex(@"$\w+^", RegexOptions.Compiled);

        public DetokenizationOperation[] Detokenize(string[] tokens)
        {
            var operations = new DetokenizationOperation[tokens.Length];

            var matchingTokens = new HashSet<string>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (!_tokenToDetokenizationOperation.ContainsKey(tokens[i]))
                {
                    operations[i] = DetokenizationOperation.NO_OPERATION;
                    continue;
                }
                
                DetokenizationOperation dictOperation = _tokenToDetokenizationOperation[tokens[i]];

                if (dictOperation == DetokenizationOperation.MERGE_TO_LEFT
                    || dictOperation == DetokenizationOperation.MERGE_TO_RIGHT
                    || dictOperation == DetokenizationOperation.MERGE_BOTH)
                {
                    operations[i] = dictOperation;
                }
                else if (dictOperation == DetokenizationOperation.MERGE_BOTH_IF_SURROUNDED_BY_WORDS)
                {
                    if(0 < i && i < tokens.Length - 1 && WordRegex.IsMatch(tokens[i-1]) && WordRegex.IsMatch(tokens[i+1])){
                        operations[i] = DetokenizationOperation.MERGE_BOTH;
                    }
                    else
                    {
                        operations[i] = DetokenizationOperation.NO_OPERATION;
                    }
                }
                else if (dictOperation == DetokenizationOperation.RIGHT_LEFT_MATCHING)
                {
                    if (matchingTokens.Contains(tokens[i]))
                    {
                        // The token already occurred once, move it to the left and clear the occurrence flag
                        operations[i] = DetokenizationOperation.MERGE_TO_LEFT;
                        matchingTokens.Remove(tokens[i]);
                    }
                    else
                    {
                        // First time this token is seen, move it to the right and remember it
                        operations[i] = DetokenizationOperation.MERGE_TO_RIGHT;
                        matchingTokens.Add(tokens[i]);
                    }
                }
                else
                {
                    throw new InvalidEnumArgumentException("Unknown operation: " + dictOperation);
                }
            }

            return operations;
        }

        public string Detokenize(string[] tokens, string splitMarker){
            DetokenizationOperation[] operations = Detokenize(tokens);

            if (tokens.Length != operations.Length)
            {
                throw new ArgumentException("tokens and operations array must have same length: tokens=" +
                                            tokens.Length + ", operations=" + operations.Length + "!");
            }

            var untokenizedString = new StringBuilder();
            for (int i = 0; i < tokens.Length; i++)
            {

                // attach token to string buffer
                untokenizedString.Append(tokens[i]);

                bool isAppendSpace;
                bool isAppendSplitMarker;

                // if this token is the last token do not attach a space
                if (i + 1 == operations.Length)
                {
                    isAppendSpace = false;
                    isAppendSplitMarker = false;
                }
                    // if next token move left, no space after this token,
                    // its safe to access next token
                else if (operations[i + 1].Equals(DetokenizationOperation.MERGE_TO_LEFT)
                            || operations[i + 1].Equals(DetokenizationOperation.MERGE_BOTH))
                {
                    isAppendSpace = false;
                    isAppendSplitMarker = true;
                }
                    // if this token is move right, no space
                else if (operations[i].Equals(DetokenizationOperation.MERGE_TO_RIGHT)
                            || operations[i].Equals(DetokenizationOperation.MERGE_BOTH))
                {
                    isAppendSpace = false;
                    isAppendSplitMarker = true;
                }
                else
                {
                    isAppendSpace = true;
                    isAppendSplitMarker = false;
                }

                if (isAppendSpace)
                {
                    untokenizedString.Append(' ');
                }

                if (isAppendSplitMarker && splitMarker != null)
                {
                    untokenizedString.Append(splitMarker);
                }
            }

            return untokenizedString.ToString();
        }

    }
}
