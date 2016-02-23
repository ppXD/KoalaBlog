BEGIN TRANSACTION

--delete BlogAccessControlXGroup
delete bacxg
from BlogAccessControlXGroup bacxg
	join BlogAccessControl bac on bac.ID = bacxg.BlogAccessControlID
	join Blog blog on blog.ID = bac.BlogID
where
	blog.Content like '%test%'

--delete AccessControl
delete bac
from BlogAccessControl bac
	join Blog blog on blog.ID = bac.BlogID
where
	blog.Content like '%test%'

--delete BlogXContent
delete bxc
from BlogXContent bxc
	join Blog blog on blog.ID = bxc.BlogID
where 
	blog.Content like '%test%'

--delete BlogXBlog
delete bxb
from BlogXBlog bxb
	join Blog blog on blog.ID = bxb.BaseBlogID
where
	blog.Content like '%test%'

--delete BlogXBlog
delete bxb
from BlogXBlog bxb
	join Blog blog on blog.ID = bxb.NewBlogID
where
	blog.Content like '%test%'

--delete CommentXComment
delete cxc
from CommentXComment cxc
	join Comment c on c.ID = cxc.BaseCommentID
where c.Content like '%test%'

--delete CommentXComment
delete cxc
from CommentXComment cxc
	join Comment c on c.ID = cxc.NewCommentID
where c.Content like '%test%'

--delete CommentXContent
delete cxc
from CommentXContent cxc
	join Comment c on c.ID = cxc.CommentID
where c.Content like '%test%'

--delete CommentXContent
delete cxc
from CommentXContent cxc
	join Content c on c.ID = cxc.ContentID
where c.ContentPath like '%test%' or c.SeoFilename like '%test%'

--delete Comment
delete c
from Comment c
	join Blog blog on blog.ID = c.BlogID
where
	c.Content like '%test%' or blog.Content like '%test%'

--delete Comment
delete c
from Comment c
where
	c.Content like '%test%'

--delete Blog
delete blog
from Blog blog
where
	blog.Content like '%test%'

--delete Content
delete content
from Content content
where
	content.ContentPath like '%test%' or content.SeoFilename like '%test%'

--delete UserAccountXPerson
delete uaxp
from UserAccountXPerson uaxp
	join UserAccount ua on ua.ID = uaxp.UserAccountID
where
	ua.UserName like '%test%' or ua.Password like '%test%' or ua.Email like '%test%'

--delete UserAccount
delete ua
from UserAccount ua
where
	ua.UserName like '%test%' or ua.Password like '%test%' or ua.Email like '%test%'

--delete GroupMember
delete gm
from GroupMember gm
	join [dbo].[Group] g on g.ID = gm.GroupID
where g.Name like '%test%'

--delete Group
delete g
from [dbo].[Group] g
where
	g.Name like '%test%'

--delete PersonXPerson
delete pxp
from PersonXPerson pxp
	join Person p on pxp.FollowerID = p.ID
where
	p.RealName like '%test%' or p.NickName like '%test%' or p.HomePage like '%test%'

--delete PersonXPerson
delete pxp
from PersonXPerson pxp
	join Person p on pxp.FollowingID = p.ID
where
	p.RealName like '%test%' or p.NickName like '%test%' or p.HomePage like '%test%'

--delete Person
delete person
from Person person
where
	person.RealName like '%test%' or person.NickName like '%test%' or person.HomePage like '%test%'

--delete Person
delete Person
from Person person
	join UserAccountXPerson uaxp on uaxp.PersonID = person.ID
where person.RealName like '%test%' or person.NickName like '%test%' or person.HomePage like '%test%'

COMMIT TRANSACTION