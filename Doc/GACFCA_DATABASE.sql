--������Ϣ������(ģ��)����ϵ(ָ��)����׼����׼��Ƭ
SELECT TOP 10 * FROM TaskCard
SELECT * FROM TaskItem WHERE TCId=1
SELECT * FROM CheckStandard WHERE TIId=1
SELECT * FROM PictureStandard WHERE TIId=1 --��׼��Ƭ

--�����ƻ���ʼѲ�꣺�ƻ�������(ģ��)�������ϵ(ָ��)�з�������׼�ϸ����ʧ����Ƭ����������÷֡�
SELECT TOP 10 * FROM Plans WHERE Id=6382
SELECT * FROM [dbo].[TaskOfPlan] WHERE Id=31053
SELECT * FROM TaskItem WHERE TCId=36
SELECT * FROM Score WHERE TPID=31053 AND ItemId=321 --��ϵ(ָ��)�з���
SELECT * FROM StandardPic WHERE TPId=31053 AND TIId=321--ʧ����Ƭ
SELECT * FROM CheckResult WHERE TPId=31053 AND TIId=321 --��׼�ϸ����
SELECT * FROM LastItemScore WHERE TPId=31053 AND TIId=321--��������÷�


--ҳ����˵�
SELECT * FROM sant.Programs WHERE ProgramId=46 --ҳ�������Ϣ
SELECT * FROM sant.Menu WHERE ProgramId=46 --�˵� AppYN �����ֻ���ʾ���
SELECT * FROM [sant].[ProgramForWeb] WHERE ProgramId=46 --Controller Action
SELECT * FROM sant.ProgramsInRoles WHERE ProgramId=46 --ҳ������Ȩ����

--Ȩ���������Ϣ
SELECT * FROM [sant].[Roles]

--��½�û���Ϣ
SELECT * FROM [sant].[Users] WHERE UserId=1--��½��
SELECT * FROM [sant].[Memberships]  WHERE UserId=1--����
SELECT * FROM [sant].[UsersInRoles]   WHERE UserId=1--��½�û�����Ȩ����

--��֯������Ϣ��
SELECT * FROM Distributor  WHERE Id=552--A:����Ա��B:ҵ�������E:������R:����Z��С����S:������
SELECT * FROM ServerOfAreas WHERE SDisId=552--��������С���Ĺ�ϵ

--��֯���½�û��Ķ�Ӧ
SELECT * FROM Employee WHERE DisId=552

--��Ա��Ϣ������
SELECT * FROM [dbo].[StaffBatch] --Ψһ�Ա�ʶ�������̡�Ѳ���·ݡ�ҵ����������֤��

--Ѳ���·���Ѳ��ľ�����
SELECT * FROM [dbo].[StaffDis]--Ψһ�Ա�ʶ�������̡�Ѳ���·ݡ�ҵ�������


--�������ݶ�Ӧ��
SELECT * FROM CodeHidden

