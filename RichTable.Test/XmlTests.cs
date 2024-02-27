using Newtonsoft.Json;
using NStandard.Text.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;

namespace RichTable.Test
{
    public class XmlTests
    {
        private static readonly string _xml = """
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<table>
	<v>
		<h>
			<c> left </c>
			<c> right </c>
			<v>
				<c> 1 </c>
				<v>
					<h>
						<c> 11 </c>
						<c> 12 </c>
						<c> 13 </c>
						<c> 14 </c>
					</h>
					<h>
						<c> 21 </c>
						<c> 22 </c>
						<c> 23 </c>
						<c> 24 </c>
					</h>
					<h>
						<c> 31 </c>
						<c> 32 </c>
						<c> 33 </c>
						<c> 34 </c>
					</h>
					<h>
						<c> 31 </c>
						<c> 32 </c>
						<c> 33 </c>
						<c> 34 </c>
					</h>
					<c> 66 </c>
				</v>
			</v>
		</h>
	</v>
</table>
"""
        ;

        [Fact]
        public void Test1()
        {
            var doc = new XmlDocument();
            doc.LoadXml(_xml);
            var text = JsonXmlSerializer.SerializeXmlNode(doc);
        }
    }
}
