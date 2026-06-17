-- ===================================================
-- EcoRAEE UAC - Script completo de Base de Datos
-- Motor: MySQL 8.0
-- Caracteres: utf8mb4
-- ===================================================

CREATE DATABASE IF NOT EXISTS EcoRaeeUac_DB
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE EcoRaeeUac_DB;

-- ===================================================
-- TABLA: Docentes
-- ===================================================
CREATE TABLE IF NOT EXISTS `Docentes` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `NombreCompleto` VARCHAR(200) NOT NULL,
    `Email` VARCHAR(200) NOT NULL,
    `Telefono` VARCHAR(20) NULL,
    `Especialidad` VARCHAR(200) NULL,
    `FechaRegistro` DATETIME(6) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE INDEX `IX_Docentes_Email` (`Email` ASC)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ===================================================
-- TABLA: Campañas Ambientales
-- ===================================================
CREATE TABLE IF NOT EXISTS `Campañas` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Nombre` VARCHAR(200) NOT NULL,
    `Fecha` DATETIME(6) NOT NULL,
    `Lugar` VARCHAR(200) NOT NULL,
    `Descripcion` LONGTEXT NOT NULL,
    `ResponsableId` INT NOT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_Campañas_ResponsableId` (`ResponsableId` ASC),
    CONSTRAINT `FK_Campañas_Docentes_ResponsableId`
        FOREIGN KEY (`ResponsableId`)
        REFERENCES `Docentes` (`Id`)
        ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ===================================================
-- TABLA: Participantes
-- ===================================================
CREATE TABLE IF NOT EXISTS `Participantes` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `NombreCompleto` VARCHAR(200) NOT NULL,
    `Rol` VARCHAR(50) NOT NULL,
    `DocumentoIdentidad` VARCHAR(20) NOT NULL,
    `CampañaAmbientalId` INT NOT NULL,
    `DocenteId` INT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_Participantes_CampañaAmbientalId` (`CampañaAmbientalId` ASC),
    INDEX `IX_Participantes_DocenteId` (`DocenteId` ASC),
    CONSTRAINT `FK_Participantes_Campañas_CampañaAmbientalId`
        FOREIGN KEY (`CampañaAmbientalId`)
        REFERENCES `Campañas` (`Id`)
        ON DELETE CASCADE,
    CONSTRAINT `FK_Participantes_Docentes_DocenteId`
        FOREIGN KEY (`DocenteId`)
        REFERENCES `Docentes` (`Id`)
        ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ===================================================
-- TABLA: Recolección RAEE
-- ===================================================
CREATE TABLE IF NOT EXISTS `Recolecciones` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `TipoResiduo` VARCHAR(100) NOT NULL,
    `Cantidad` DOUBLE NOT NULL,
    `LugarRecoleccion` VARCHAR(200) NOT NULL,
    `Fecha` DATETIME(6) NOT NULL,
    `Co2EvitadoKg` DOUBLE NOT NULL DEFAULT 0,
    `MaterialRecuperadoKg` DOUBLE NOT NULL DEFAULT 0,
    `ArbolesSalvados` INT NOT NULL DEFAULT 0,
    `EnergiaAhorradaKwh` DOUBLE NOT NULL DEFAULT 0,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ===================================================
-- TABLA: Material Educativo
-- ===================================================
CREATE TABLE IF NOT EXISTS `MaterialesEducativos` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Titulo` VARCHAR(200) NOT NULL,
    `Tipo` VARCHAR(30) NOT NULL,
    `RutaArchivo` LONGTEXT NULL,
    `EnlaceUrl` LONGTEXT NULL,
    `FechaPublicacion` DATETIME(6) NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ===================================================
-- DATOS DE PRUEBA (Seed Data)
-- ===================================================

-- Docentes
INSERT INTO `Docentes` (`NombreCompleto`, `Email`, `Telefono`, `Especialidad`, `FechaRegistro`) VALUES
('Pendiente de Asignar', 'pendiente@uac.edu.pe', NULL, NULL, NOW()),
('Dr. Carlos Mendoza López', 'cmendoza@uac.edu.pe', '984123456', 'Ingeniería Ambiental', NOW()),
('MSc. Ana Torres García', 'atorres@uac.edu.pe', '984234567', 'Responsabilidad Social', NOW()),
('Dr. Pedro Castillo Ramos', 'pcastillo@uac.edu.pe', '984345678', 'Gestión de Residuos', NOW()),
('MSc. Lucia Fernández Paz', 'lfernandez@uac.edu.pe', '984456789', 'Educación Ambiental', NOW()),
('Dr. Jorge Martínez Ríos', 'jmartinez@uac.edu.pe', '984567890', 'Desarrollo Sostenible', NOW());

-- Campañas
INSERT INTO `Campañas` (`Nombre`, `Fecha`, `Lugar`, `Descripcion`, `ResponsableId`) VALUES
('RAEE-tech: Recicla tu electrónico', '2026-04-15', 'Hall Principal - Pabellón A', 'Campaña de recolección de residuos electrónicos para toda la comunidad universitaria. Se recibirán computadoras, monitores, teclados, celulares y cables.', 2),
('Semana Verde UAC 2026', '2026-05-20', 'Campus Central', 'Jornada de concientización ambiental con talleres, charlas y puntos de recolección de RAEE en todo el campus universitario.', 3),
('Puntos Limpios Móviles', '2026-06-10', 'Estacionamiento Principal', 'Instalación de contenedores móviles para la recepción de aparatos eléctricos y electrónicos en desuso de la comunidad universitaria.', 4),
('Eco-Reto: Menos basura electrónica', '2026-07-05', 'Auditorio Central', 'Competencia entre facultades para ver quién recolecta la mayor cantidad de RAEE. La facultad ganadora recibirá un reconocimiento institucional.', 5),
('Capacitación en Gestión de RAEE', '2026-08-12', 'Sala de Conferencias - Pabellón B', 'Taller de capacitación dirigido a docentes y estudiantes sobre la correcta disposición de residuos electrónicos y su impacto ambiental.', 6);

-- Participantes
INSERT INTO `Participantes` (`NombreCompleto`, `Rol`, `DocumentoIdentidad`, `CampañaAmbientalId`, `DocenteId`) VALUES
('María Quispe Hancco', 'Estudiante', 'DNI-76543210', 1, 2),
('José Luis Huamán Rivas', 'Estudiante', 'DNI-76543211', 1, 2),
('Carmen Rosa Sotomayor', 'Docente', 'DNI-76543212', 1, 2),
('Ricardo Palma Quispe', 'Estudiante', 'DNI-76543213', 2, 3),
('Sofía Condori Mamani', 'Estudiante', 'DNI-76543214', 2, 3),
('Miguel Ángel Torres', 'Ciudadano Beneficiario', 'DNI-76543215', 2, NULL),
('Lucía Pumacahua Ccama', 'Estudiante', 'DNI-76543216', 3, 4),
('Diego Alejandro Luna', 'Estudiante', 'DNI-76543217', 3, 4),
('Rosa María Ortiz', 'Docente', 'DNI-76543218', 3, 4),
('Fernando Castro Díaz', 'Estudiante', 'DNI-76543219', 4, 5),
('Andrea Nicole Paredes', 'Estudiante', 'DNI-76543220', 4, 5),
('Jorge Luis Mamani', 'Ciudadano Beneficiario', 'DNI-76543221', 4, NULL),
('Valeria Alessandra Ríos', 'Estudiante', 'DNI-76543222', 5, 6),
('Renato Gabriel Silva', 'Estudiante', 'DNI-76543223', 5, 6),
('Katherine Paucar Huillca', 'Estudiante', 'DNI-76543224', 5, 6),
('Daniela Estrada Pacheco', 'Docente', 'DNI-76543225', 5, 6),
('Pedro Infanzón Soria', 'Estudiante', 'DNI-76543226', 1, 2),
('Luz Marina Ccoyo', 'Ciudadano Beneficiario', 'DNI-76543227', 2, NULL),
('Humberto Taco Ccama', 'Estudiante', 'DNI-76543228', 3, 4),
('Gianella Quispe Flores', 'Estudiante', 'DNI-76543229', 4, 5);

-- Recolecciones
INSERT INTO `Recolecciones` (`TipoResiduo`, `Cantidad`, `LugarRecoleccion`, `Fecha`, `Co2EvitadoKg`, `MaterialRecuperadoKg`, `ArbolesSalvados`, `EnergiaAhorradaKwh`) VALUES
('Computadoras', 45.50, 'Hall Principal - Pabellón A', '2026-04-15', 38.68, 42.32, 0, 113.75),
('Monitores', 30.20, 'Hall Principal - Pabellón A', '2026-04-15', 25.67, 28.09, 0, 75.50),
('Celulares', 12.80, 'Hall Principal - Pabellón A', '2026-04-15', 10.88, 11.90, 0, 32.00),
('Teclados', 8.40, 'Estacionamiento Principal', '2026-06-10', 7.14, 7.81, 0, 21.00),
('Cables', 25.30, 'Campus Central', '2026-05-20', 21.51, 23.53, 0, 63.25),
('Computadoras', 60.00, 'Campus Central', '2026-05-20', 51.00, 55.80, 0, 150.00),
('Impresoras', 35.70, 'Auditorio Central', '2026-07-05', 30.35, 33.20, 0, 89.25),
('Baterías', 5.50, 'Sala de Conferencias', '2026-08-12', 4.68, 5.12, 0, 13.75),
('Computadoras', 28.60, 'Estacionamiento Principal', '2026-06-10', 24.31, 26.60, 0, 71.50),
('Pequeños Electrodomésticos', 15.40, 'Auditorio Central', '2026-07-05', 13.09, 14.32, 0, 38.50);

-- Material Educativo
INSERT INTO `MaterialesEducativos` (`Titulo`, `Tipo`, `RutaArchivo`, `EnlaceUrl`, `FechaPublicacion`) VALUES
('Guía rápida: ¿Qué son los RAEE?', 'Infografía', NULL, 'https://www.minam.gob.pe/calidad-ambiental/raee/', '2026-03-01'),
('Video: Reciclaje de Electrónicos', 'Video', NULL, 'https://www.youtube.com/watch?v=example1', '2026-03-15'),
('Manual de Gestión de Residuos Electrónicos', 'PDF', NULL, 'https://www.minam.gob.pe/wp-content/uploads/2023/05/guia-raee.pdf', '2026-04-01'),
('Beneficios del Reciclaje de RAEE', 'Infografía', NULL, 'https://example.com/beneficios-raee', '2026-04-10'),
('Noticia: UAC lidera campaña de reciclaje', 'Noticia', NULL, 'https://www.uac.edu.pe/noticias/campana-reciclaje', '2026-04-20'),
('Conferencia: Economía Circular y RAEE', 'Video', NULL, 'https://www.youtube.com/watch?v=example2', '2026-05-05'),
('Infografía: Ciclo de Vida de un Electrónico', 'Infografía', NULL, 'https://example.com/ciclo-vida-electronico', '2026-05-15'),
('Reglamento de Gestión de RAEE en Perú', 'PDF', NULL, 'https://www.minam.gob.pe/wp-content/uploads/2022/01/decreto-supremo-raee.pdf', '2026-06-01');

-- ===================================================
-- FIN DEL SCRIPT
-- ===================================================
