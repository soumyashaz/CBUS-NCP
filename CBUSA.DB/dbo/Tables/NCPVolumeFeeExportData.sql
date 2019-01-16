CREATE TABLE [dbo].[NCPVolumeFeeExportData] (
    [TransactionId]          VARCHAR (20)    NOT NULL,
    [ProgramId]              VARCHAR (20)    NOT NULL,
    [Year]                   INT             NOT NULL,
    [Quarter]                INT             NOT NULL,
    [BuilderId]              BIGINT          NOT NULL,
    [BuilderRebate]          DECIMAL (18, 2) NOT NULL,
    [CBUSAVolumeFee]         DECIMAL (18, 2) NOT NULL,
    [DataGenerationDateTime] DATETIME        NULL
);

