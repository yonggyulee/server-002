INSERT INTO workflow.worker (id, workflow_type, job_type, server_id, cpu_count, cpu_memory, gpu_count, gpu_memory, properties, description)
VALUES  ('TestWorker', 'RECIPE' , 'DEFAULT', 'TestServer', 100, 1000000, 100, 1000000, NULL, NULL),
        ('TestWorker2', 'PIPELINE' , 'DEFAULT', 'TestServer', 100, 1000000, 100, 1000000, NULL, NULL)