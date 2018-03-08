--基础信息：任务卡(模块)←体系(指标)←标准←标准照片
SELECT TOP 10 * FROM TaskCard
SELECT * FROM TaskItem WHERE TCId=1
SELECT * FROM CheckStandard WHERE TIId=1
SELECT * FROM PictureStandard WHERE TIId=1 --标准照片

--制作计划开始巡店：计划←任务卡(模块)：针对体系(指标)有分数、标准合格与否、失分照片、近期明检得分。
SELECT TOP 10 * FROM Plans WHERE Id=6382
SELECT * FROM [dbo].[TaskOfPlan] WHERE Id=31053
SELECT * FROM TaskItem WHERE TCId=36
SELECT * FROM Score WHERE TPID=31053 AND ItemId=321 --体系(指标)有分数
SELECT * FROM StandardPic WHERE TPId=31053 AND TIId=321--失分照片
SELECT * FROM CheckResult WHERE TPId=31053 AND TIId=321 --标准合格与否
SELECT * FROM LastItemScore WHERE TPId=31053 AND TIId=321--近期明检得分


--页面与菜单
SELECT * FROM sant.Programs WHERE ProgramId=46 --页面基础信息
SELECT * FROM sant.Menu WHERE ProgramId=46 --菜单 AppYN 控制手机显示与否
SELECT * FROM [sant].[ProgramForWeb] WHERE ProgramId=46 --Controller Action
SELECT * FROM sant.ProgramsInRoles WHERE ProgramId=46 --页面所在权限组

--权限组基础信息
SELECT * FROM [sant].[Roles]

--登陆用户信息
SELECT * FROM [sant].[Users] WHERE UserId=1--登陆名
SELECT * FROM [sant].[Memberships]  WHERE UserId=1--密码
SELECT * FROM [sant].[UsersInRoles]   WHERE UserId=1--登陆用户所在权限组

--组织基础信息表
SELECT * FROM Distributor  WHERE Id=552--A:管理员，B:业务分区，E:大区，R:区域，Z：小区，S:经销商
SELECT * FROM ServerOfAreas WHERE SDisId=552--经销商与小区的关系

--组织与登陆用户的对应
SELECT * FROM Employee WHERE DisId=552

--人员信息基础表
SELECT * FROM [dbo].[StaffBatch] --唯一性标识：经销商、巡检月份、业务分区、身份证。

--巡检月份下巡检的经销商
SELECT * FROM [dbo].[StaffDis]--唯一性标识：经销商、巡检月份、业务分区。


--基础数据对应表
SELECT * FROM CodeHidden

