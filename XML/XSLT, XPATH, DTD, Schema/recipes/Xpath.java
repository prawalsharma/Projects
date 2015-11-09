/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package xpath;

import javax.xml.xpath.*;
import org.w3c.dom.*;
import org.xml.sax.InputSource;

public class Xpath {

   static void print ( Node e ) {
		if (e instanceof Text)
			System.out.print(((Text) e).getData());
		else {
			NodeList c = e.getChildNodes();
			System.out.print("<"+e.getNodeName());
			NamedNodeMap attributes = e.getAttributes();
			for (int i = 0; i < attributes.getLength(); i++)
				System.out.print(" "+attributes.item(i).getNodeName()
					+"=\""+attributes.item(i).getNodeValue()+"\"");
			System.out.print(">");
			for (int k = 0; k < c.getLength(); k++)
				print(c.item(k));
			System.out.print("</"+e.getNodeName()+">");
		}
	}

    static void eval ( String query, String document ) throws Exception {
		XPathFactory xpathFactory = XPathFactory.newInstance();
		XPath xpath = xpathFactory.newXPath();
		InputSource inputSource = new InputSource(document);
		NodeList result = (NodeList) xpath.evaluate(query,inputSource,XPathConstants.NODESET);
		System.out.println("XPath query: "+query);
		for (int i = 0; i < result.getLength(); i++)
			print(result.item(i));
		System.out.println();
    }

    public static void main ( String[] args ) throws Exception {
        
        //eval("/*","SigmodRecord.xml");
        System.out.println("Query 1: Title of all the articles whose one of the authors is David Maier:");
        eval("//article[authors/author='David Maier']/title","SigmodRecord.xml");
        
         System.out.println("Query 2: Title of all the articles whose first author is David Maier:");
        eval("//article[authors/author[1]='David Maier']/title","SigmodRecord.xml");
        
         System.out.println("Query 3: Title of all the articles whose authors include David Maier and Stanley B. Zdonik:");
        eval("//article[authors/author='David Maier' and authors/author='Stanley B. Zdonik']/title","SigmodRecord.xml");
    
         System.out.println("Query 4: Title of all the articles within volume19/number2:");
        eval("//issue[volume='19' and number='2']/articles/article/title","SigmodRecord.xml");
           
        System.out.println("Query 5: Titles and init/end pages of all articles in volume19/number2 whose authors include Jim Gray:");
        eval("//issue[volume='19' and number='2']/articles/article[authors//author='Jim Gray']/*[self::title or self::initPage or self::endPage]","SigmodRecord.xml");
        
        System.out.println("Query 6: Volume and Number of all articles whose authors include David Maier:");
        eval("//issue[articles/article/authors/author='David Maier']/*[self::volume or self::number]","SigmodRecord.xml");
    
    }
    
}
