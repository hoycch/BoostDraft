using System;
using System.Collections.Generic;

namespace SimpleXMLValidatorLibrary
{
    public static class SimpleXmlValidator
    {
        public static bool DetermineXml(string xmlContent)
        {
            try
            {
                Stack<string> tagStack = new Stack<string>();
                int i = 0;
                while (i < xmlContent.Length)
                {
                    if (xmlContent[i] == '<')
                    {
                        if (i + 1 < xmlContent.Length && xmlContent[i + 1] == '/')
                        {
                            // Closing tag
                            int closingTagStart = i + 2;
                            int closingTagEnd = xmlContent.IndexOf('>', closingTagStart);
                            if (closingTagEnd == -1) return false;
                            string tagName = xmlContent.Substring(closingTagStart, closingTagEnd - closingTagStart);
                            if (tagStack.Count == 0 || tagStack.Pop() != tagName) return false;
                            i = closingTagEnd + 1;
                        }
                        else
                        {
                            // Opening or self-closing tag
                            int tagStart = i + 1;
                            int tagEnd = xmlContent.IndexOfAny(new char[] { ' ', '>', '/' }, tagStart);
                            if (tagEnd == -1) return false;
                            string tagName = xmlContent.Substring(tagStart, tagEnd - tagStart);

                            if (xmlContent[tagEnd] == '/')
                            {
                                // Self-closing tag
                                if (xmlContent[tagEnd + 1] != '>') return false;
                                i = tagEnd + 2;
                            }
                            else if (xmlContent[tagEnd] == '>')
                            {
                                // Opening tag
                                tagStack.Push(tagName);
                                i = tagEnd + 1;
                            }
                            else
                            {
                                // Attributes in the tag
                                int endOfTag = xmlContent.IndexOf('>', tagEnd);
                                if (endOfTag == -1) return false;
                                if (xmlContent[endOfTag - 1] == '/')
                                {
                                    // Self-closing tag with attributes
                                    i = endOfTag + 1;
                                }
                                else
                                {
                                    // Opening tag with attributes
                                    tagStack.Push(tagName);
                                    i = endOfTag + 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                return tagStack.Count == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
