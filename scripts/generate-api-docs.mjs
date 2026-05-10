#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const repoRoot = path.resolve(__dirname, '..');
const githubBase = 'https://github.com/adz/FsFlow/blob/main';

const targets = [
  path.join(repoRoot, 'docs', 'reference'),
];

const pageSpecs = [
  {
    outPath: ['fsflow', 'flow.md'],
    title: 'Flow',
    description: 'Source-documented workflow surface in FsFlow.',
    intro:
      "This page shows the source-documented `Flow` surface: the core type, module functions, and computation expressions.",
    symbols: [
      { section: 'Core type', ids: ['T:FsFlow.Flow`3'] },
      { section: 'Builder', ids: ['P:FsFlow.Builders.flow'] },
      { section: 'Module functions', ids: ['M:FsFlow.Flow.run', 'M:FsFlow.Flow.ok', 'M:FsFlow.Flow.error', 'M:FsFlow.Flow.succeed', 'M:FsFlow.Flow.value', 'M:FsFlow.Flow.fail', 'M:FsFlow.Flow.fromResult', 'M:FsFlow.Flow.fromOption', 'M:FsFlow.Flow.fromValueOption', 'M:FsFlow.Flow.orElseFlow', 'M:FsFlow.Flow.env', 'M:FsFlow.Flow.read', 'M:FsFlow.Flow.map', 'M:FsFlow.Flow.bind', 'M:FsFlow.Flow.tap', 'M:FsFlow.Flow.tapError', 'M:FsFlow.Flow.mapError', 'M:FsFlow.Flow.catch', 'M:FsFlow.Flow.orElseWith', 'M:FsFlow.Flow.orElse', 'M:FsFlow.Flow.zip', 'M:FsFlow.Flow.map2', 'M:FsFlow.Flow.map3', 'M:FsFlow.Flow.apply', 'M:FsFlow.Flow.ignore', 'M:FsFlow.Flow.localEnv', 'M:FsFlow.Flow.provideLayer', 'M:FsFlow.Flow.delay', 'M:FsFlow.Flow.traverse', 'M:FsFlow.Flow.sequence'] },
      { section: 'Concurrency', ids: ['T:FsFlow.Fiber`2', 'M:FsFlow.Flow.fork', 'M:FsFlow.Flow.join', 'M:FsFlow.Flow.interrupt'] },
      { section: 'Parallel orchestration', ids: ['M:FsFlow.Flow.zipPar', 'M:FsFlow.Flow.race'] },
      { section: 'Scheduling', ids: ['T:FsFlow.Schedule`3', 'M:FsFlow.Schedule.recurs', 'M:FsFlow.Schedule.spaced', 'M:FsFlow.Schedule.exponential', 'M:FsFlow.Schedule.jittered', 'M:FsFlow.FlowScheduleExtensions.Flow`3.Retry.Static', 'M:FsFlow.FlowScheduleExtensions.Flow`3.Repeat.Static'] },
    ],
  },
  {
    outPath: ['fsflow', 'check.md'],
    title: 'Check',
    description: 'Source-documented pure predicate helpers for FsFlow.',
    intro:
      'This page shows the source-documented `Check` surface: the unit-failure result type and reusable predicate helpers.',
    symbols: [
      { section: 'Core type', ids: ['T:FsFlow.Check`1'] },
      { section: 'Module functions', ids: ['M:FsFlow.Check.fromPredicate', 'M:FsFlow.Check.not', 'M:FsFlow.Check.and', 'M:FsFlow.Check.or', 'M:FsFlow.Check.all', 'M:FsFlow.Check.any', 'M:FsFlow.Check.okIf', 'M:FsFlow.Check.failIf', 'M:FsFlow.Check.okIfSome', 'M:FsFlow.Check.okIfNone', 'M:FsFlow.Check.failIfSome', 'M:FsFlow.Check.failIfNone', 'M:FsFlow.Check.okIfValueSome', 'M:FsFlow.Check.okIfValueNone', 'M:FsFlow.Check.failIfValueSome', 'M:FsFlow.Check.failIfValueNone', 'M:FsFlow.Check.okIfNotNull', 'M:FsFlow.Check.okIfNull', 'M:FsFlow.Check.failIfNotNull', 'M:FsFlow.Check.failIfNull', 'M:FsFlow.Check.okIfNotEmpty', 'M:FsFlow.Check.okIfEmpty', 'M:FsFlow.Check.failIfNotEmpty', 'M:FsFlow.Check.failIfEmpty', 'M:FsFlow.Check.okIfEqual', 'M:FsFlow.Check.okIfNotEqual', 'M:FsFlow.Check.failIfEqual', 'M:FsFlow.Check.failIfNotEqual', 'M:FsFlow.Check.okIfNonEmptyStr', 'M:FsFlow.Check.okIfEmptyStr', 'M:FsFlow.Check.failIfNonEmptyStr', 'M:FsFlow.Check.failIfEmptyStr', 'M:FsFlow.Check.okIfNotBlank', 'M:FsFlow.Check.notBlank', 'M:FsFlow.Check.okIfBlank', 'M:FsFlow.Check.blank', 'M:FsFlow.Check.failIfNotBlank', 'M:FsFlow.Check.failIfBlank', 'M:FsFlow.Check.orError', 'M:FsFlow.Check.orErrorWith', 'M:FsFlow.Check.notNull', 'M:FsFlow.Check.notEmpty', 'M:FsFlow.Check.equal', 'M:FsFlow.Check.notEqual'] },
    ],
  },
  {
    outPath: ['fsflow', 'validation.md'],
    title: 'Validation',
    description: 'Source-documented accumulating validation for FsFlow.',
    intro:
      'This page shows the source-documented `Validation` surface: the accumulating result type, module functions, path-scoping helpers, and the `validate { }` builder.',
    symbols: [
      { section: 'Core type', ids: ['T:FsFlow.Validation`2'] },
      { section: 'Builder', ids: ['P:FsFlow.Builders.validate'] },
      { section: 'Module functions', ids: ['M:FsFlow.Validation.toResult', 'M:FsFlow.Validation.ok', 'M:FsFlow.Validation.error', 'M:FsFlow.Validation.succeed', 'M:FsFlow.Validation.fail', 'M:FsFlow.Validation.fromResult', 'M:FsFlow.Validation.map', 'M:FsFlow.Validation.bind', 'M:FsFlow.Validation.mapError', 'M:FsFlow.Validation.map2', 'M:FsFlow.Validation.map3', 'M:FsFlow.Validation.apply', 'M:FsFlow.Validation.ignore', 'M:FsFlow.Validation.orElse', 'M:FsFlow.Validation.orElseWith', 'M:FsFlow.Validation.collect', 'M:FsFlow.Validation.sequence', 'M:FsFlow.Validation.traverseIndexed', 'M:FsFlow.Validation.merge'] },
      { section: 'Path scoping', ids: ['M:FsFlow.Validation.at', 'M:FsFlow.Validation.key', 'M:FsFlow.Validation.index', 'M:FsFlow.Validation.name'] },
    ],
  },
  {
    outPath: ['fsflow', 'capability.md'],
    title: 'Capability',
    description: 'Source-documented capabilities and layers for FsFlow.',
    intro:
      'This page shows the source-documented capability and layer surface, including CAPS request tokens and environment management helpers.',
    symbols: [
      { section: 'CAPS tokens', ids: ['T:FsFlow.Needs`1', 'T:FsFlow.Env`1', 'T:FsFlow.Env`2'] },
      { section: 'Capabilities', ids: ['T:FsFlow.MissingCapability', 'M:FsFlow.Capability.service', 'M:FsFlow.Capability.runtime', 'M:FsFlow.Capability.environment', 'M:FsFlow.Capability.serviceFromProvider'] },
      { section: 'Layers', ids: ['M:FsFlow.Layer.provideLayer'] },
    ],
  },
];

function cleanXmlDocText(text) {
  if (!text) return '';
  return text
    .replace(/<c>([\s\S]*?)<\/c>/gi, '`$1`')
    .replace(/<code>([\s\S]*?)<\/code>/gi, (_match, code) => {
      const trimmed = code.trim();
      return `\n\n\`\`\`fsharp\n${trimmed}\n\`\`\`\n\n`;
    })
    .replace(/<paramref name="([^"]+)"\s*\/>/gi, '`$1`')
    .replace(/<see cref="([^"]+)"\s*\/>/gi, (_match, cref) => {
      const withoutPrefix = cref.replace(/^[A-Z]:/, '');
      const lastSegment = withoutPrefix.split(/[.:]/).pop() ?? withoutPrefix;
      return `\`${lastSegment.replace(/`[0-9]+/g, '')}\``;
    })
    .trim();
}

function parseXmlDocs() {
  const docs = new Map();
  const xmlFiles = [];
  
  const walk = (dir) => {
    if (!fs.existsSync(dir)) return;
    const files = fs.readdirSync(dir);
    for (const file of files) {
      const fullPath = path.join(dir, file);
      if (fs.statSync(fullPath).isDirectory()) {
        walk(fullPath);
      } else if (file.endsWith('.xml') && (fullPath.includes('debug_net8.0') || fullPath.includes('debug_netstandard2.1'))) {
        xmlFiles.push(fullPath);
      }
    }
  };

  walk(path.join(repoRoot, 'artifacts', 'bin'));

  for (const xmlFile of xmlFiles) {
    const content = fs.readFileSync(xmlFile, 'utf8');
    const memberMatches = content.matchAll(/<member name=\"([^\"]+)\">([\s\S]*?)<\/member>/g);
    for (const match of memberMatches) {
      const name = match[1];
      const inner = match[2];
      
      const summary = cleanXmlDocText(inner.match(/<summary>([\s\S]*?)<\/summary>/)?.[1]);
      const remarks = cleanXmlDocText(inner.match(/<remarks>([\s\S]*?)<\/remarks>/)?.[1]);
      const returns = cleanXmlDocText(inner.match(/<returns>([\s\S]*?)<\/returns>/)?.[1]);
      
      const params = [];
      const paramMatches = inner.matchAll(/<param name=\"([^\"]+)\">([\s\S]*?)<\/param>/g);
      for (const p of paramMatches) {
        params.push({ name: p[1], description: cleanXmlDocText(p[2]) });
      }

      const examples = [];
      const exampleMatches = inner.matchAll(/<example>([\s\S]*?)<\/example>/g);
      for (const e of exampleMatches) {
        examples.push(cleanXmlDocText(e[1]));
      }

      docs.set(name, { summary, remarks, returns, params, examples });
    }
  }
  return docs;
}

function getShortName(id) {
  const parts = id.replace(/^[A-Z]:/, '').split('(')[0].split('.');
  let last = parts.pop();
  if (last.startsWith('get_')) last = last.substring(4);
  return last.replace(/`[0-9]+/g, '');
}

function getQualifiedName(id) {
  let name = id.replace(/^[A-Z]:/, '').split('(')[0];
  if (name.includes('.get_')) name = name.replace('.get_', '.');
  return name.replace(/`[0-9]+/g, '');
}

function getPageName(id) {
  return `${getQualifiedName(id).toLowerCase().split('.').join('-')}.md`;
}

function renderSymbolPage(id, doc) {
  const shortName = getShortName(id);
  const qualifiedName = getQualifiedName(id);
  
  let content = `---
title: ${qualifiedName}
linkTitle: ${shortName}
---

${doc.summary || ''}

${doc.remarks ? `## Remarks\n\n${doc.remarks}\n` : ''}

`;

  if (doc.params.length > 0) {
    content += `## Parameters\n\n`;
    for (const p of doc.params) {
      content += `- \`${p.name}\`: ${p.description}\n`;
    }
    content += '\n';
  }

  if (doc.returns) {
    content += `## Returns\n\n${doc.returns}\n\n`;
  }

  if (doc.examples.length > 0) {
    content += `## Examples\n\n`;
    for (const example of doc.examples) {
      content += `${example}\n\n`;
    }
  }

  return content;
}

function generate() {
  const allDocs = parseXmlDocs();
  
  for (const targetRoot of targets) {
    for (const spec of pageSpecs) {
      const outPath = path.join(targetRoot, ...spec.outPath);
      fs.mkdirSync(path.dirname(outPath), { recursive: true });
      
      let pageContent = `---
title: ${spec.title}
---

${spec.intro}

`;

      for (const section of spec.symbols) {
        pageContent += `## ${section.section}\n\n`;
        for (const id of section.ids) {
          // Exact match or prefix match (for overloaded methods)
          let matchId = null;
          const candidates = [
            id,
            id.replace(/^([TMFP]):FsFlow\.([^\.]+)\./, '$1:FsFlow.$2Module.'),
            id.replace(/^([TMFP]):FsFlow\.([^\.]+)\./, '$1:FsFlow.$2Builders.'),
            id.replace(/^([TMFP]):FsFlow\.([^\.]+)\./, '$1:FsFlow.$2Extensions.'),
            id.replace(/^([TMFP]):FsFlow\.([^\.]+)\./, '$1:FsFlow.Flow$2Extensions.'),
            id.replace(/^P:FsFlow\.([^\.]+)\./, 'M:FsFlow.$1.get_'), // Getter check
            id.replace(/^P:FsFlow\.([^\.]+)\./, 'M:FsFlow.$1Module.get_'),
          ];

          for (const cand of candidates) {
            if (allDocs.has(cand)) {
              matchId = cand;
              break;
            }
            for (const key of allDocs.keys()) {
              if (key.startsWith(cand + '(') || key.startsWith(cand + '`')) {
                matchId = key;
                break;
              }
            }
            if (matchId) break;
          }

          const doc = matchId ? allDocs.get(matchId) : null;
          if (doc) {
            const pageName = getPageName(matchId);
            const qualifiedName = getQualifiedName(matchId);
            pageContent += `- [\`${qualifiedName}\`](./${pageName}): ${doc.summary || ''}\n`;
            
            const symbolPagePath = path.join(path.dirname(outPath), pageName);
            fs.writeFileSync(symbolPagePath, renderSymbolPage(matchId, doc), 'utf8');
          } else {
            console.warn(`Warning: Missing doc for ${id}`);
            pageContent += `- \`${getQualifiedName(id)}\` (undocumented)\n`;
          }
        }
        pageContent += '\n';
      }
      
      fs.writeFileSync(outPath, pageContent, 'utf8');
    }
  }
}

generate();
