 


 SET IDENTITY_INSERT dbo.elected_officials ON
 GO

 INSERT INTO dbo.elected_officials
 (ID, Name, Office, Committee, Salary, PersonalPage, DisclosureLink, ImageUrl)
     VALUES
	 (1, 'Bill Peduto', 'Mayor', 'People for Peduto', 111843.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1487_Peduto.pdf',
                           'http://pittsburghpa.gov/mayor/mayor-profile.html', 'peduto.jpg'),
         (2, 'Michael Lamb', 'City Controller', 'Committee to Elect Michael Lamb', 73917.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1490_Lamb.pdf',
                           'http://pittsburghpa.gov/controller/about/co-bio.html', 'controller.jpg'),
         (3, 'Darlene Harris', 'City Council District 1', 'Darlene Harris Election Committee', 66371.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1492_Harris.pdf',
                           'http://pittsburghpa.gov/council/district1/harris.html', 'harris.jpg'),
            (4, 'Theresa Kail-Smith', 'City Council District 2', 'Friends to Elect Theresa Smith', 66371.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1486_Smith.pdf',
                           'http://pittsburghpa.gov/council/district2/kail-smith.html', 'smith.jpg'),
           (5, 'Bruce Kraus', 'City Council District 3', 'Friends of Bruce Kraus', 66371.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1491_Kraus.pdf',
                           'http://pittsburghpa.gov/council/district3/kraus.html', 'kraus.jpg'),
            (6, 'Anthony Coghill', 'City Council District 4', 'Coghill for City Council', 66371.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1493_Coghill.pdf',
                           'http://pittsburghpa.gov/council/district4/contacts.html', 'Anthony_Coghill.jpg'),
            (7, 'Corey O’Connor', 'City Council District 5', 'Friends of Corey O’Connor', 66371.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1488_O''Connor.pdf',
                           'http://pittsburghpa.gov/council/district5/oconnor.html', 'oconnor.jpg'),
            (8, 'Daniel Lavelle', 'City Council District 6', 'Citizens for Daniel Lavelle', 66371.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1489_Lavelle.pdf',
                           'http://pittsburghpa.gov/council/district6/lavelle.html', 'lavelle.jpg'),
            (9, 'Deborah Gross', 'City Council District 7', 'Friends of Deb Gross', 66371.0,
                           'http://apps.pittsburghpa.gov/co/Gross_Dist_7.pdf',
                           'http://pittsburghpa.gov/council/district7/gross.html', 'gross.jpg'),
           (10, 'Erika Strassburger', 'City Council District 8', 'Friends of Erika', 66371.0, null,
                           'http://pittsburghpa.gov/council/district8/contacts.html', 'erika_strassburger_pittsburgh.jpg'),
            (11, 'Ricky Burgess', 'City Council District 9', 'Friends of Ricky Burgess', 66371.0,
                           'http://apps.pittsburghpa.gov/redtail/images/1494_Burgess.pdf',
                           'http://pittsburghpa.gov/council/district9/burgess.html', 'burgess.jpg');

	GO